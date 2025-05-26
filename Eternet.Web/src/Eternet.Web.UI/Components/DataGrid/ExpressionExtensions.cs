using System.Linq.Expressions;
using System.Reflection;
using Eternet.Web.UI;
using Eternet.Web.UI.Abstractions.DataGrid;
using Eternet.Web.UI.Abstractions.Services;

namespace Invoices.Incoming.WebApi.Components.DataGrid;

//TODO: Test this class
public static class ExpressionExtensions
{
    private class ParameterReplacer : ExpressionVisitor
    {
        public ParameterExpression Source = null!;
        public ParameterExpression Target = null!;

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == Source ? Target : base.VisitParameter(node);
        }
    }

    public static Expression ReplaceParameter(Expression expression, ParameterExpression source, ParameterExpression target)
    {
        return new ParameterReplacer { Source = source, Target = target }.Visit(expression);
    }

    public static Expression? CombineExpressions(this Expression? left, Expression? right)
    {
        if (left == null)
        {
            return right;
        }

        return right == null ? left : Expression.OrElse(left, right);
    }

    public static Expression? BuildColumnExpression<TValue>(
        this LambdaExpression lambdaExpression,
        ParameterExpression parameter,
        IEnumerable<TValue> values,
        Func<Expression, TValue, Expression> comparisonExpressionBuilder)
    {
        if (!values.Any())
        {
            return null;
        }

        var columnBody = ReplaceParameter(lambdaExpression.Body, lambdaExpression.Parameters[0], parameter);
        Expression? columnExpression = null;

        foreach (var value in values)
        {
            var comparisonExpression = comparisonExpressionBuilder(columnBody, value);

            columnExpression = columnExpression == null
                ? comparisonExpression
                : Expression.OrElse(columnExpression, comparisonExpression);
        }

        return columnExpression;
    }

    public static Expression BuildEqualExpression<TValue>(
        this Expression columnBody,
        TValue value)
    {
        var valueExpression = Expression.Constant(value, typeof(TValue));
        return Expression.Equal(columnBody, valueExpression);
    }

    public static bool IsColumnOfType(this LambdaExpression propertyExpression, Type expectedType)
    {
        var returnType = propertyExpression.Body.Type;

        return returnType == expectedType ||
               (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == expectedType);
    }

    public static Type GetPropertyType(this LambdaExpression propertyExpression)
    {
        return propertyExpression.Body.Type;
    }

    public static bool IsColumnNumericType(this LambdaExpression propertyExpression)
    {
        var returnType = propertyExpression.Body.Type;
        return
            returnType == typeof(int) ||
            returnType == typeof(long) ||
            returnType == typeof(short) ||
            returnType == typeof(double) ||
            returnType == typeof(decimal) ||
            returnType == typeof(float);
    }
}

public static class MethodInfoExtensions
{
    public static MethodInfo ToLowerMethod { get; } = typeof(string).GetMethod("ToLower", Type.EmptyTypes)!;
    public static MethodInfo SimpleContainsMethod { get; } = typeof(string).GetMethod("Contains", [typeof(string)])!;
    public static MethodInfo SimpleStartsWithMethod { get; } = typeof(string).GetMethod("StartsWith", [typeof(string)])!;
    public static MethodInfo SimpleEndsWithMethod { get; } = typeof(string).GetMethod("EndsWith", [typeof(string)])!;
}

public static class FilterExtensions
{
    public static Expression? GenerateStringFilterExpression(this Expression propertyAccessExpression, StringFilters stringFilters)
    {
        Expression? currentColumnFilterExpression;
        Expression? innerCombinedExpression = null;

        foreach (var condition in stringFilters.Conditions.Where(c => !string.IsNullOrWhiteSpace(c.Value)))
        {
            var filterText = condition.Value ?? "";
            Expression targetPropertyExpr = propertyAccessExpression;
            Expression filterConstantExpr;

            var notNullPropertyExpr = Expression.NotEqual(propertyAccessExpression, Expression.Constant(null, typeof(string)));

            if (condition.CaseInsensitive)
            {
                targetPropertyExpr = Expression.Call(propertyAccessExpression, MethodInfoExtensions.ToLowerMethod);
                filterConstantExpr = Expression.Constant(filterText.ToLower());
            }
            else
            {
                targetPropertyExpr = propertyAccessExpression;
                filterConstantExpr = Expression.Constant(filterText);
            }

            Expression? comparisonExpression = null;
            switch (condition.Operator)
            {
                case FilterStringOperator.Contains:
                    comparisonExpression = Expression.Call(targetPropertyExpr, MethodInfoExtensions.SimpleContainsMethod, filterConstantExpr);
                    break;
                case FilterStringOperator.StartsWith:
                    comparisonExpression = Expression.Call(targetPropertyExpr, MethodInfoExtensions.SimpleStartsWithMethod, filterConstantExpr);
                    break;
                case FilterStringOperator.EndsWith:
                    comparisonExpression = Expression.Call(targetPropertyExpr, MethodInfoExtensions.SimpleEndsWithMethod, filterConstantExpr);
                    break;
                case FilterStringOperator.Equals:
                    comparisonExpression = Expression.Equal(targetPropertyExpr, filterConstantExpr);
                    break;
            }

            if (comparisonExpression != null)
            {
                var conditionExpression = Expression.AndAlso(notNullPropertyExpr, comparisonExpression);

                innerCombinedExpression = innerCombinedExpression == null
                    ? conditionExpression
                    : (stringFilters.CombineOperator == CombineOperator.And
                        ? Expression.AndAlso(innerCombinedExpression, conditionExpression)
                        : Expression.OrElse(innerCombinedExpression, conditionExpression));
            }
        }
        currentColumnFilterExpression = innerCombinedExpression;
        return currentColumnFilterExpression;
    }

    public static Expression GenerateBooleanFilterExpression(this Expression propertyAccessExpression, bool boolValue)
    {
        Expression? currentColumnFilterExpression;
        Expression filterConstant;
        if (propertyAccessExpression.Type == typeof(bool?))
        {
            filterConstant = Expression.Constant(boolValue, typeof(bool?));
            var hasValueExpr = Expression.Property(propertyAccessExpression, nameof(Nullable<bool>.HasValue));
            var valueExpr = Expression.Property(propertyAccessExpression, nameof(Nullable<bool>.Value));
            var valueEqualsExpr = Expression.Equal(valueExpr, Expression.Constant(boolValue));
            currentColumnFilterExpression = Expression.AndAlso(hasValueExpr, valueEqualsExpr);
        }
        else
        {
            filterConstant = Expression.Constant(boolValue, typeof(bool));
            currentColumnFilterExpression = Expression.Equal(propertyAccessExpression, filterConstant);
        }

        return currentColumnFilterExpression;
    }

    public static (HashSet<int> IntValues, HashSet<long> LongValues, HashSet<string> StringValues) ParseGeneralFilter(
        this string generalFilter)
    {
        var intValues = new HashSet<int>();
        var longValues = new HashSet<long>();
        var stringValues = new HashSet<string>();

        if (string.IsNullOrWhiteSpace(generalFilter))
        {
            return (intValues, longValues, stringValues);
        }

        var values = generalFilter.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        foreach (var value in values)
        {
            if (int.TryParse(value, out var intValue))
            {
                intValues.Add(intValue);
            }
            else if (long.TryParse(value, out var longValue))
            {
                longValues.Add(longValue);
            }
            else
            {
                stringValues.Add(value);
            }
        }

        return (intValues, longValues, stringValues);
    }

    public static Expression? CreateFilterExpression<TGridItem>(
        this GridColumnMetadata<TGridItem> column,
        IBuildStringLikeExpression buildStringLikeExpression,
        (HashSet<int> IntValues, HashSet<long> LongValues, HashSet<string> StringValues) parsedFilter,
        HashSet<string> allValuesAsStrings,
        Expression? combinedExpression,
        ParameterExpression parameter,
        bool ignoreCase = false)
    {
        var lambdaExpression = column.LambdaExpression;
        if (lambdaExpression.IsColumnOfType(typeof(string)))
        {
            var values = column.AllowsIntegerFiltering
                ? allValuesAsStrings
                : parsedFilter.StringValues;
            Func<Expression, string, Expression> buildFunc = (columnBody, value) =>
            {
                if (ignoreCase)
                {
                    var loweredColumnBody = Expression.Call(columnBody, MethodInfoExtensions.ToLowerMethod);
                    var lowerValue = value.ToLower();
                    return buildStringLikeExpression.Build(loweredColumnBody, lowerValue);
                }
                else
                {
                    return buildStringLikeExpression.Build(columnBody, value);
                }
            };

            var columnExpression = lambdaExpression.BuildColumnExpression(
                parameter,
                values,
                buildFunc);

            combinedExpression = combinedExpression.CombineExpressions(columnExpression);
        }
        else if (lambdaExpression.IsColumnOfType(typeof(int)))
        {
            var columnExpression = lambdaExpression.BuildColumnExpression(
                parameter,
                parsedFilter.IntValues,
                (columnBody, value) => columnBody.BuildEqualExpression(value)
            );

            combinedExpression = combinedExpression.CombineExpressions(columnExpression);
        }
        else if (lambdaExpression.IsColumnOfType(typeof(long)))
        {
            var columnExpression = lambdaExpression.BuildColumnExpression(
                parameter,
                parsedFilter.LongValues,
                (columnBody, value) => columnBody.BuildEqualExpression(value)
            );

            combinedExpression = combinedExpression.CombineExpressions(columnExpression);
        }

        return combinedExpression;
    }
}

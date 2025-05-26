using System.Linq.Expressions;
using Microsoft.OData.UriParser;

namespace Eternet.Accounting.Api.Services;

internal static class BinaryOperatorNodeExtensions
{

    private static BinaryExpression CombineBinary(this BinaryOperatorNode bin,
                                            ParameterExpression p,
                                            PropertyInfo[] props)
    {
        var left = BuildPredicate(bin.Left, p, props);
        var right = BuildPredicate(bin.Right, p, props);

        return bin.OperatorKind switch
        {
            BinaryOperatorKind.And => Expression.AndAlso(left, right),
            BinaryOperatorKind.Or => Expression.OrElse(left, right),
            _ => throw new NotSupportedException($"Operator {bin.OperatorKind} not supported")
        };
    }

    public static Expression BuildPredicate(this QueryNode node,
                                             ParameterExpression p,
                                             PropertyInfo[] props)
    {
        return node switch
        {
            SearchTermNode term => BuildTerm(term.Text, p, props),
            BinaryOperatorNode bin => bin.CombineBinary(p, props),
            UnaryOperatorNode unary => Expression.Not(unary.Operand.BuildPredicate(p, props)),
            _ => throw new NotSupportedException($"$search node {node.Kind} not supported")
        };
    }

    private static readonly MethodInfo s_like = typeof(DbFunctionsExtensions)
    .GetMethod(nameof(DbFunctionsExtensions.Like),
               [typeof(DbFunctions), typeof(string), typeof(string)])!;

    private static Expression BuildTerm(string term,
                                    ParameterExpression p,
                                    PropertyInfo[] props)
    {
        if (props.Length == 0)
        {
            return Expression.Constant(true);
        }

        var ef = Expression.Property(null, typeof(EF), nameof(EF.Functions));
        var pattern = Expression.Constant($"%{term}%");

        Expression? body = null;

        foreach (var prop in props)
        {
            var propExpr = Expression.Property(p, prop);
            var notNull = Expression.NotEqual(propExpr, Expression.Constant(null, typeof(string)));
            var like = Expression.Call(s_like, ef, propExpr, pattern);
            var clause = Expression.AndAlso(notNull, like);

            body = body is null ? clause : Expression.OrElse(body, clause);
        }

        return body!;
    }

}

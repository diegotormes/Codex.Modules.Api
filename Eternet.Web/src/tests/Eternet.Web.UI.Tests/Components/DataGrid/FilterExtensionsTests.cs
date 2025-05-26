using System.Linq.Expressions;
using FluentAssertions;
using Invoices.Incoming.WebApi.Components.DataGrid;
using Eternet.Web.UI.Abstractions.DataGrid;
using Eternet.Web.UI.Abstractions.Services;

namespace Eternet.Web.UI.Tests.Components.DataGrid;

public class FilterExtensionsTests
{
    private class TestItem
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public bool? Active { get; set; }
    }

    private class TestBuilder : IBuildStringLikeExpression
    {
        public List<string> Values { get; } = new();

        public Expression Build(Expression columnBody, string value)
        {
            Values.Add(value);
            return Expression.Call(columnBody, MethodInfoExtensions.SimpleContainsMethod, Expression.Constant(value));
        }
    }

    [Fact]
    public void GenerateStringFilterExpression_ReturnsContainsExpression()
    {
        var param = Expression.Parameter(typeof(TestItem), "x");
        var property = Expression.Property(param, nameof(TestItem.Name));
        var filters = new StringFilters
        {
            Conditions =
            [
                new FilterCondition { Value = "foo", Operator = FilterStringOperator.Contains }
            ]
        };

        var expr = property.GenerateStringFilterExpression(filters);
        var func = Expression.Lambda<Func<TestItem, bool>>(expr!, param).Compile();

        func(new TestItem { Name = "foo" }).Should().BeTrue();
        func(new TestItem { Name = "bar" }).Should().BeFalse();
    }

    [Fact]
    public void GenerateStringFilterExpression_CaseInsensitive_Works()
    {
        var param = Expression.Parameter(typeof(TestItem), "x");
        var property = Expression.Property(param, nameof(TestItem.Name));
        var filters = new StringFilters
        {
            Conditions =
            [
                new FilterCondition
                {
                    Value = "FOO",
                    Operator = FilterStringOperator.Contains,
                    CaseInsensitive = true
                }
            ]
        };

        var expr = property.GenerateStringFilterExpression(filters);
        var func = Expression.Lambda<Func<TestItem, bool>>(expr!, param).Compile();

        func(new TestItem { Name = "foo" }).Should().BeTrue();
    }

    [Fact]
    public void GenerateBooleanFilterExpression_ForNullableBool_ReturnsExpression()
    {
        var param = Expression.Parameter(typeof(TestItem), "x");
        var property = Expression.Property(param, nameof(TestItem.Active));

        var expr = property.GenerateBooleanFilterExpression(true);
        var func = Expression.Lambda<Func<TestItem, bool>>(expr, param).Compile();

        func(new TestItem { Active = true }).Should().BeTrue();
        func(new TestItem { Active = false }).Should().BeFalse();
        func(new TestItem { Active = null }).Should().BeFalse();
    }

    [Fact]
    public void CreateFilterExpression_ForStringColumn_ReturnsCombinedExpression()
    {
        Expression<Func<TestItem, string?>> lambda = i => i.Name;
        var propertyParam = lambda.Parameters[0];
        var meta = new GridColumnMetadata<TestItem>
        {
            ColumnName = nameof(TestItem.Name),
            Title = "Name",
            AllowsSorting = false,
            AllowsSearching = true,
            AllowsFiltering = true,
            AllowsIntegerFiltering = false,
            LambdaExpression = lambda,
            PropertyExpression = Expression.Lambda<Func<TestItem, object>>(Expression.Convert(lambda.Body, typeof(object)), propertyParam),
            FilterableAttribute = null
        };
        var builder = new TestBuilder();
        var parsed = (new HashSet<int>(), new HashSet<long>(), new HashSet<string> { "foo" });
        var allValues = new HashSet<string> { "foo" };
        var param = Expression.Parameter(typeof(TestItem), "p");

        var expr = meta.CreateFilterExpression(builder, parsed, allValues, null, param, false)!;
        builder.Values.Should().ContainSingle("foo");
        var func = Expression.Lambda<Func<TestItem, bool>>(expr, param).Compile();

        func(new TestItem { Name = "foo" }).Should().BeTrue();
        func(new TestItem { Name = "bar" }).Should().BeFalse();
    }

    [Fact]
    public void CreateFilterExpression_ForIntColumn_ReturnsCombinedExpression()
    {
        Expression<Func<TestItem, int>> lambda = i => i.Age;
        var propertyParam = lambda.Parameters[0];
        var meta = new GridColumnMetadata<TestItem>
        {
            ColumnName = nameof(TestItem.Age),
            Title = "Age",
            AllowsSorting = false,
            AllowsSearching = true,
            AllowsFiltering = true,
            AllowsIntegerFiltering = false,
            LambdaExpression = lambda,
            PropertyExpression = Expression.Lambda<Func<TestItem, object>>(Expression.Convert(lambda.Body, typeof(object)), propertyParam),
            FilterableAttribute = null
        };
        var builder = new TestBuilder();
        var parsed = (new HashSet<int> { 1, 2 }, new HashSet<long>(), new HashSet<string>());
        var allValues = new HashSet<string>();
        var param = Expression.Parameter(typeof(TestItem), "p");

        var expr = meta.CreateFilterExpression(builder, parsed, allValues, null, param, false)!;
        var func = Expression.Lambda<Func<TestItem, bool>>(expr, param).Compile();

        func(new TestItem { Age = 1 }).Should().BeTrue();
        func(new TestItem { Age = 2 }).Should().BeTrue();
        func(new TestItem { Age = 3 }).Should().BeFalse();
    }
}

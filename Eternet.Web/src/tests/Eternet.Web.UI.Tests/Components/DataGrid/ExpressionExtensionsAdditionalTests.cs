using System.Linq.Expressions;
using FluentAssertions;
using Invoices.Incoming.WebApi.Components.DataGrid;

namespace Eternet.Web.UI.Tests.Components.DataGrid;

public class ExpressionExtensionsAdditionalTests
{
    private class TestItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    [Fact]
    public void ReplaceParameter_ReplacesParameterInExpression()
    {
        var oldParam = Expression.Parameter(typeof(TestItem), "x");
        var newParam = Expression.Parameter(typeof(TestItem), "y");
        var expr = Expression.Property(oldParam, nameof(TestItem.Id));

        var replaced = ExpressionExtensions.ReplaceParameter(expr, oldParam, newParam);
        var lambda = Expression.Lambda<Func<TestItem, int>>(replaced, newParam).Compile();

        lambda(new TestItem { Id = 7 }).Should().Be(7);
    }

    [Fact]
    public void BuildColumnExpression_WithValues_BuildsOrExpression()
    {
        var lambdaParam = Expression.Parameter(typeof(TestItem), "x");
        Expression<Func<TestItem, int>> lambda = t => t.Id;
        var param = Expression.Parameter(typeof(TestItem), "p");

        var expr = lambda.BuildColumnExpression(
            param,
            [1, 2],
            (body, value) => body.BuildEqualExpression(value));
        var predicate = Expression.Lambda<Func<TestItem, bool>>(expr!, param).Compile();

        predicate(new TestItem { Id = 1 }).Should().BeTrue();
        predicate(new TestItem { Id = 2 }).Should().BeTrue();
        predicate(new TestItem { Id = 3 }).Should().BeFalse();
    }

    [Fact]
    public void BuildColumnExpression_EmptyValues_ReturnsNull()
    {
        var param = Expression.Parameter(typeof(TestItem), "p");
        Expression<Func<TestItem, int>> lambda = t => t.Id;

        var expr = lambda.BuildColumnExpression(
            param,
            Array.Empty<int>(),
            (body, value) => body.BuildEqualExpression(value));

        expr.Should().BeNull();
    }

    [Fact]
    public void IsColumnOfType_ReturnsTrueForMatchingType()
    {
        Expression<Func<TestItem, string?>> lambda = t => t.Name;

        var result = lambda.IsColumnOfType(typeof(string));

        result.Should().BeTrue();
    }

    [Fact]
    public void GetPropertyType_ReturnsType()
    {
        Expression<Func<TestItem, int>> lambda = t => t.Id;

        var result = lambda.GetPropertyType();

        result.Should().Be(typeof(int));
    }

    [Fact]
    public void IsColumnNumericType_DetectsNumericTypes()
    {
        Expression<Func<TestItem, int>> lambda = t => t.Id;

        var result = lambda.IsColumnNumericType();

        result.Should().BeTrue();
    }
}

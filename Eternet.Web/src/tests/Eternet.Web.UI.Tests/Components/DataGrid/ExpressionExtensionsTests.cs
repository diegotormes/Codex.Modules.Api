using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Invoices.Incoming.WebApi.Components.DataGrid;
using Eternet.Web.UI;
using Eternet.Web.UI.Abstractions.DataGrid;
using Eternet.Web.UI.Abstractions.Services;
using Microsoft.JSInterop;

namespace Eternet.Web.UI.Tests.Components.DataGrid;

public class ExpressionExtensionsTests
{
    private class TestItem
    {
        public int Id { get; set; }
        public long LongId { get; set; }
        public string? Name { get; set; }
        public bool? Active { get; set; }
        public bool Enabled { get; set; }
        public DateTime? Created { get; set; }
        public DateTime Modified { get; set; }
    }

    private class DummyViewsService : IViewsService
    {
        public IQueryable<T> GetViewAsQuery<T>() where T : class => Enumerable.Empty<T>().AsQueryable();
        public Task<bool> UpdateSourcePropertyAsync(object id, Type entityType, string propertyName, object newValue) => Task.FromResult(false);
        public Task<bool> UpdateViewPropertyAsync(object id, Type entityType, string propertyName, object newValue) => Task.FromResult(false);
    }

    private class DummyBuilder : IBuildStringLikeExpression
    {
        public Expression Build(Expression columnBody, string value) => Expression.Constant(true);
    }

    private class DummyGridColumnService : IGridColumnService
    {
        public Dictionary<string, GridColumnMetadata<TGridItem>> GetColumnsFor<TGridItem>() => new();
    }

    private class DummyJsRuntime : IJSRuntime
    {
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => ValueTask.FromResult(default(TValue)!);
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) => ValueTask.FromResult(default(TValue)!);
    }

    [Fact]
    public void CombineExpressions_LeftIsNull_ReturnsRight()
    {
        Expression? left = null;
        var param = Expression.Parameter(typeof(TestItem), "t");
        Expression right = Expression.Equal(Expression.Property(param, nameof(TestItem.Id)), Expression.Constant(1));

        var result = left.CombineExpressions(right);

        result.Should().Be(right);
    }

    [Fact]
    public void CombineExpressions_RightIsNull_ReturnsLeft()
    {
        var param = Expression.Parameter(typeof(TestItem), "t");
        Expression left = Expression.Equal(Expression.Property(param, nameof(TestItem.Id)), Expression.Constant(1));
        Expression? right = null;

        var result = left.CombineExpressions(right);

        result.Should().Be(left);
    }

    [Fact]
    public void CombineExpressions_BothNotNull_ReturnsOrElseExpression()
    {
        var param = Expression.Parameter(typeof(TestItem), "t");
        Expression left = Expression.Equal(Expression.Property(param, nameof(TestItem.Id)), Expression.Constant(1));
        Expression right = Expression.Equal(Expression.Property(param, nameof(TestItem.Id)), Expression.Constant(2));

        var result = left.CombineExpressions(right) as BinaryExpression;

        result!.NodeType.Should().Be(ExpressionType.OrElse);
        result.Left.Should().Be(left);
        result.Right.Should().Be(right);
    }

    [Fact]
    public void BuildEqualExpression_CreatesCorrectEquality()
    {
        var param = Expression.Parameter(typeof(TestItem), "t");
        Expression column = Expression.Property(param, nameof(TestItem.Id));

        var expr = column.BuildEqualExpression(5);
        var lambda = Expression.Lambda<Func<TestItem, bool>>(expr, param).Compile();

        lambda(new TestItem { Id = 5 }).Should().BeTrue();
        lambda(new TestItem { Id = 1 }).Should().BeFalse();
    }

    [Fact]
    public void ParseGeneralFilter_ParsesNumbersAndStrings()
    {
        const string filter = "1,2,3000000000,test,foo";

        var (ints, longs, strings) = filter.ParseGeneralFilter();

        ints.Should().Equal([1, 2]);
        longs.Should().Equal([3000000000L]);
        strings.Should().Equal(["test", "foo"]);
    }

    [Fact]
    public void GenerateBooleanFilterExpression_ForNullableBool_Works()
    {
        var param = Expression.Parameter(typeof(TestItem), "t");
        var property = Expression.Property(param, nameof(TestItem.Active));

        var expr = property.GenerateBooleanFilterExpression(true);
        var func = Expression.Lambda<Func<TestItem, bool>>(expr, param).Compile();

        func(new TestItem { Active = true }).Should().BeTrue();
        func(new TestItem { Active = false }).Should().BeFalse();
        func(new TestItem { Active = null }).Should().BeFalse();
    }

    [Fact]
    public void GenerateBooleanFilterExpression_ForBool_Works()
    {
        var param = Expression.Parameter(typeof(TestItem), "t");
        var property = Expression.Property(param, nameof(TestItem.Enabled));

        var expr = property.GenerateBooleanFilterExpression(false);
        var func = Expression.Lambda<Func<TestItem, bool>>(expr, param).Compile();

        func(new TestItem { Enabled = false }).Should().BeTrue();
        func(new TestItem { Enabled = true }).Should().BeFalse();
    }

    [Fact]
    public void ApplyColumnFilters_DateTimeFilters_WorkCorrectly()
    {
        var views = new DummyViewsService();
        var builder = new DummyBuilder();
        var columnService = new DummyGridColumnService();
        var js = new DummyJsRuntime();
        var toast = new DummyToastService();

        var grid = new EternetDataGrid<TestItem>(views, builder, columnService, js, toast);

        Expression<Func<TestItem, DateTime?>> createdExp = i => i.Created;
        Expression<Func<TestItem, DateTime>> modifiedExp = i => i.Modified;

        var metaCreated = new GridColumnMetadata<TestItem>
        {
            ColumnName = nameof(TestItem.Created),
            Title = "Created",
            AllowsSorting = false,
            AllowsSearching = false,
            AllowsFiltering = true,
            AllowsIntegerFiltering = false,
            LambdaExpression = createdExp,
            PropertyExpression = Expression.Lambda<Func<TestItem, object>>(Expression.Convert(createdExp.Body, typeof(object)), createdExp.Parameters[0]),
            FilterableAttribute = null
        };

        var metaModified = new GridColumnMetadata<TestItem>
        {
            ColumnName = nameof(TestItem.Modified),
            Title = "Modified",
            AllowsSorting = false,
            AllowsSearching = false,
            AllowsFiltering = true,
            AllowsIntegerFiltering = false,
            LambdaExpression = modifiedExp,
            PropertyExpression = Expression.Lambda<Func<TestItem, object>>(Expression.Convert(modifiedExp.Body, typeof(object)), modifiedExp.Parameters[0]),
            FilterableAttribute = null
        };

        var all = new Dictionary<string, GridColumnMetadata<TestItem>>
        {
            [nameof(TestItem.Created)] = metaCreated,
            [nameof(TestItem.Modified)] = metaModified
        };

        var targetCreated = new DateTime(2023, 1, 1);
        var targetModified = new DateTime(2023, 12, 25);

        var filters = new Dictionary<string, object?>
        {
            [nameof(TestItem.Created)] = targetCreated,
            [nameof(TestItem.Modified)] = targetModified
        };

        var t = grid.GetType();
        t.GetField("_allColumns", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(grid, all);
        t.GetField("_filterableColumns", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(grid, all);
        t.GetField("_columnFilters", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(grid, filters);

        var method = t.GetMethod("ApplyColumnFilters", BindingFlags.NonPublic | BindingFlags.Instance)!;

        var items = new[]
        {
            new TestItem { Created = targetCreated, Modified = targetModified },
            new TestItem { Created = targetCreated.AddDays(1), Modified = targetModified },
            new TestItem { Created = null, Modified = targetModified.AddDays(1) }
        }.AsQueryable();

        var result = (IQueryable<TestItem>)method.Invoke(grid, [items])!;
        var list = result.ToList();

        list.Should().ContainSingle();
        list[0].Created.Should().Be(targetCreated);
        list[0].Modified.Should().Be(targetModified);
    }
}

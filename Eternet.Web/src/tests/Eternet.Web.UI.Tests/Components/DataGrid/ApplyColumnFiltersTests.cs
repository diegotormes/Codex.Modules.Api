using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Eternet.Web.UI.Abstractions.DataGrid;
using Eternet.Web.UI.Abstractions.Services;
using Microsoft.JSInterop;

namespace Eternet.Web.UI.Tests.Components.DataGrid;

public class ApplyColumnFiltersTests
{
    private enum Status { A, B }

    private class Item
    {
        public int Age { get; set; }
        public Status Status { get; set; }
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
    public void ApplyColumnFilters_FiltersNumericAndEnum()
    {
        var views = new DummyViewsService();
        var builder = new DummyBuilder();
        var columnService = new DummyGridColumnService();
        var js = new DummyJsRuntime();
        var toast = new DummyToastService();

        var grid = new EternetDataGrid<Item>(views, builder, columnService, js, toast);

        Expression<Func<Item, int>> ageExp = i => i.Age;
        Expression<Func<Item, Status>> statusExp = i => i.Status;

        var metaAge = new GridColumnMetadata<Item>
        {
            ColumnName = nameof(Item.Age),
            Title = "Age",
            AllowsSorting = false,
            AllowsSearching = false,
            AllowsFiltering = true,
            AllowsIntegerFiltering = false,
            LambdaExpression = ageExp,
            PropertyExpression = Expression.Lambda<Func<Item, object>>(Expression.Convert(ageExp.Body, typeof(object)), ageExp.Parameters[0]),
            FilterableAttribute = null
        };
        var metaStatus = new GridColumnMetadata<Item>
        {
            ColumnName = nameof(Item.Status),
            Title = "Status",
            AllowsSorting = false,
            AllowsSearching = false,
            AllowsFiltering = true,
            AllowsIntegerFiltering = false,
            LambdaExpression = statusExp,
            PropertyExpression = Expression.Lambda<Func<Item, object>>(Expression.Convert(statusExp.Body, typeof(object)), statusExp.Parameters[0]),
            FilterableAttribute = null
        };

        var all = new Dictionary<string, GridColumnMetadata<Item>>
        {
            [nameof(Item.Age)] = metaAge,
            [nameof(Item.Status)] = metaStatus
        };
        var filters = new Dictionary<string, object?>
        {
            [nameof(Item.Age)] = new NumericFilter(30, FilterNumericOperator.GreaterOrEqual),
            [nameof(Item.Status)] = Status.A
        };

        var t = grid.GetType();
        t.GetField("_allColumns", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(grid, all);
        t.GetField("_filterableColumns", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(grid, all);
        t.GetField("_columnFilters", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(grid, filters);

        var method = t.GetMethod("ApplyColumnFilters", BindingFlags.NonPublic | BindingFlags.Instance)!;

        var items = new[]
        {
            new Item { Age = 40, Status = Status.A },
            new Item { Age = 20, Status = Status.A },
            new Item { Age = 35, Status = Status.B },
        }.AsQueryable();

        var result = (IQueryable<Item>)method.Invoke(grid, [items])!;
        var list = result.ToList();

        list.Should().ContainSingle();
        list[0].Age.Should().Be(40);
        list[0].Status.Should().Be(Status.A);
    }

    [Fact]
    public void ApplyColumnFilters_LessOrEqualOperator()
    {
        var views = new DummyViewsService();
        var builder = new DummyBuilder();
        var columnService = new DummyGridColumnService();
        var js = new DummyJsRuntime();
        var toast = new DummyToastService();

        var grid = new EternetDataGrid<Item>(views, builder, columnService, js, toast);

        Expression<Func<Item, int>> ageExp = i => i.Age;
        Expression<Func<Item, Status>> statusExp = i => i.Status;

        var metaAge = new GridColumnMetadata<Item>
        {
            ColumnName = nameof(Item.Age),
            Title = "Age",
            AllowsSorting = false,
            AllowsSearching = false,
            AllowsFiltering = true,
            AllowsIntegerFiltering = false,
            LambdaExpression = ageExp,
            PropertyExpression = Expression.Lambda<Func<Item, object>>(Expression.Convert(ageExp.Body, typeof(object)), ageExp.Parameters[0]),
            FilterableAttribute = null
        };
        var metaStatus = new GridColumnMetadata<Item>
        {
            ColumnName = nameof(Item.Status),
            Title = "Status",
            AllowsSorting = false,
            AllowsSearching = false,
            AllowsFiltering = true,
            AllowsIntegerFiltering = false,
            LambdaExpression = statusExp,
            PropertyExpression = Expression.Lambda<Func<Item, object>>(Expression.Convert(statusExp.Body, typeof(object)), statusExp.Parameters[0]),
            FilterableAttribute = null
        };

        var all = new Dictionary<string, GridColumnMetadata<Item>>
        {
            [nameof(Item.Age)] = metaAge,
            [nameof(Item.Status)] = metaStatus
        };
        var filters = new Dictionary<string, object?>
        {
            [nameof(Item.Age)] = new NumericFilter(30, FilterNumericOperator.LessOrEqual),
            [nameof(Item.Status)] = Status.A
        };

        var t = grid.GetType();
        t.GetField("_allColumns", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(grid, all);
        t.GetField("_filterableColumns", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(grid, all);
        t.GetField("_columnFilters", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(grid, filters);

        var method = t.GetMethod("ApplyColumnFilters", BindingFlags.NonPublic | BindingFlags.Instance)!;

        var items = new[]
        {
            new Item { Age = 40, Status = Status.A },
            new Item { Age = 20, Status = Status.A },
            new Item { Age = 35, Status = Status.B },
        }.AsQueryable();

        var result = (IQueryable<Item>)method.Invoke(grid, [items])!;
        var list = result.ToList();

        list.Should().ContainSingle();
        list[0].Age.Should().Be(20);
        list[0].Status.Should().Be(Status.A);
    }

    [Fact]
    public void ApplyColumnFilters_EqualsOperator()
    {
        var views = new DummyViewsService();
        var builder = new DummyBuilder();
        var columnService = new DummyGridColumnService();
        var js = new DummyJsRuntime();
        var toast = new DummyToastService();

        var grid = new EternetDataGrid<Item>(views, builder, columnService, js, toast);

        Expression<Func<Item, int>> ageExp = i => i.Age;
        Expression<Func<Item, Status>> statusExp = i => i.Status;

        var metaAge = new GridColumnMetadata<Item>
        {
            ColumnName = nameof(Item.Age),
            Title = "Age",
            AllowsSorting = false,
            AllowsSearching = false,
            AllowsFiltering = true,
            AllowsIntegerFiltering = false,
            LambdaExpression = ageExp,
            PropertyExpression = Expression.Lambda<Func<Item, object>>(Expression.Convert(ageExp.Body, typeof(object)), ageExp.Parameters[0]),
            FilterableAttribute = null
        };
        var metaStatus = new GridColumnMetadata<Item>
        {
            ColumnName = nameof(Item.Status),
            Title = "Status",
            AllowsSorting = false,
            AllowsSearching = false,
            AllowsFiltering = true,
            AllowsIntegerFiltering = false,
            LambdaExpression = statusExp,
            PropertyExpression = Expression.Lambda<Func<Item, object>>(Expression.Convert(statusExp.Body, typeof(object)), statusExp.Parameters[0]),
            FilterableAttribute = null
        };

        var all = new Dictionary<string, GridColumnMetadata<Item>>
        {
            [nameof(Item.Age)] = metaAge,
            [nameof(Item.Status)] = metaStatus
        };
        var filters = new Dictionary<string, object?>
        {
            [nameof(Item.Age)] = new NumericFilter(35, FilterNumericOperator.Equals),
            [nameof(Item.Status)] = Status.B
        };

        var t = grid.GetType();
        t.GetField("_allColumns", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(grid, all);
        t.GetField("_filterableColumns", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(grid, all);
        t.GetField("_columnFilters", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(grid, filters);

        var method = t.GetMethod("ApplyColumnFilters", BindingFlags.NonPublic | BindingFlags.Instance)!;

        var items = new[]
        {
            new Item { Age = 40, Status = Status.A },
            new Item { Age = 20, Status = Status.A },
            new Item { Age = 35, Status = Status.B },
        }.AsQueryable();

        var result = (IQueryable<Item>)method.Invoke(grid, [items])!;
        var list = result.ToList();

        list.Should().ContainSingle();
        list[0].Age.Should().Be(35);
        list[0].Status.Should().Be(Status.B);
    }
}

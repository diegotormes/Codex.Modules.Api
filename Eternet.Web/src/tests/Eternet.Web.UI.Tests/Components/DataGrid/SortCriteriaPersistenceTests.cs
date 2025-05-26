using System.Linq.Expressions;
using Bunit;
using FluentAssertions;
using Eternet.Web.UI.Abstractions.Services;
using Eternet.Web.UI.Abstractions.DataGrid;
using Eternet.Web.UI.Components.DataGrid.Models;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Eternet.Web.UI.Tests.Components.DataGrid;

public class SortCriteriaPersistenceTests : Bunit.TestContext
{
    private class Item
    {
        public string First { get; set; } = string.Empty;
    }

    private class DummyViewsService : IViewsService
    {
        public IQueryable<T> GetViewAsQuery<T>() where T : class => Enumerable.Empty<T>().AsQueryable();
        public Task<bool> UpdateSourcePropertyAsync(object id, Type entityType, string propertyName, object newValue) => Task.FromResult(true);
        public Task<bool> UpdateViewPropertyAsync(object id, Type entityType, string propertyName, object newValue) => Task.FromResult(true);
    }

    private class DummyBuilder : IBuildStringLikeExpression
    {
        public Expression Build(Expression columnBody, string value) => columnBody;
    }

    private class DummyGridColumnService : IGridColumnService
    {
        public Dictionary<string, GridColumnMetadata<TGridItem>> GetColumnsFor<TGridItem>()
        {
            var prop = typeof(Item).GetProperty(nameof(Item.First))!;
            var param = Expression.Parameter(typeof(TGridItem), "x");
            var body = Expression.Property(param, prop);
            var lambda = Expression.Lambda(body, param);
            var lambdaObj = Expression.Lambda<Func<TGridItem, object>>(Expression.Convert(body, typeof(object)), param);

            var meta = new GridColumnMetadata<TGridItem>
            {
                ColumnName = prop.Name,
                Title = prop.Name,
                AllowsSorting = true,
                AllowsSearching = true,
                AllowsFiltering = false,
                AllowsIntegerFiltering = false,
                LambdaExpression = lambda,
                PropertyExpression = lambdaObj,
                FilterableAttribute = null
            };
            return new Dictionary<string, GridColumnMetadata<TGridItem>> { [prop.Name] = meta };
        }
    }

    [Fact]
    public async Task ApplyConfigurationAsync_RestoresSortCriteria()
    {
        Services.AddFluentUIComponents();
        JSInterop.Mode = JSRuntimeMode.Loose;
        var grid = new EternetDataGrid<Item>(new DummyViewsService(), new DummyBuilder(), new DummyGridColumnService(), JSInterop.JSRuntime, new DummyToastService());

        const System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
        typeof(EternetDataGrid<Item>).GetMethod("OnInitialized", flags)!.Invoke(grid, null);
        typeof(EternetDataGrid<Item>).GetField("_grid", flags)!.SetValue(grid, new FluentDataGrid<Item>());

        var config = new EternetDataGridConfiguration
        {
            Name = "Cfg",
            ColumnStates = [ new EternetDataGridColumnState { ColumnName = "First", Title = "First", IsVisible = true } ],
            SortCriteria = [ ("First", SortDirection.Descending) ]
        };

        var method = typeof(EternetDataGrid<Item>).GetMethod("ApplyConfigurationAsync", flags)!;
        var task = (Task)method.Invoke(grid, [config])!;
        await task;

        var criteriaField = typeof(EternetDataGrid<Item>).GetField("_sortCriteria", flags)!;
        var criteria = (List<(string Column, SortDirection Direction)>)criteriaField.GetValue(grid)!;

        criteria.Should().ContainSingle().And.Contain(("First", SortDirection.Descending));
    }
}


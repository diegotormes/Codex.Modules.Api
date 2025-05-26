using System.Linq.Expressions;
using Bunit;
using FluentAssertions;
using Eternet.Web.UI.Abstractions.DataGrid;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Eternet.Web.UI.Tests.Components.DataGrid;

public class AdvancedFilterPanelTests : Bunit.TestContext
{
    private class Item
    {
        public string? Name { get; set; }
    }

    private static Dictionary<string, GridColumnMetadata<Item>> CreateColumns()
    {
        Expression<Func<Item, string?>> exp = i => i.Name;
        var param = exp.Parameters[0];
        var meta = new GridColumnMetadata<Item>
        {
            ColumnName = nameof(Item.Name),
            Title = "Name",
            AllowsSorting = false,
            AllowsSearching = false,
            AllowsFiltering = true,
            AllowsIntegerFiltering = false,
            LambdaExpression = exp,
            PropertyExpression = Expression.Lambda<Func<Item, object>>(Expression.Convert(exp.Body, typeof(object)), param),
            FilterableAttribute = null
        };
        return new Dictionary<string, GridColumnMetadata<Item>> { [meta.ColumnName] = meta };
    }

    [Fact]
    public void ApplyButtonClick_InvokesCallback()
    {
        var columns = CreateColumns();
        var filters = new Dictionary<string, object?> { [nameof(Item.Name)] = null };
        var called = false;
        Services.AddFluentUIComponents();
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = RenderComponent<AdvancedFilterPanel<Item>>(p => p
            .Add(x => x.FilterableColumns, columns)
            .Add(x => x.ColumnFilters, filters)
            .Add(x => x.ApplyFiltersAndRefreshAsync, () => { called = true; return Task.CompletedTask; })
            .Add(x => x.ClearFiltersAsync, () => Task.CompletedTask)
        );

        cut.Find("fluent-button[aria-label='Aplicar filtros']").Click();

        called.Should().BeTrue();
    }

    [Fact]
    public void ClearButtonClick_InvokesCallback()
    {
        var columns = CreateColumns();
        var filters = new Dictionary<string, object?> { [nameof(Item.Name)] = null };
        var called = false;
        Services.AddFluentUIComponents();
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = RenderComponent<AdvancedFilterPanel<Item>>(p => p
            .Add(x => x.FilterableColumns, columns)
            .Add(x => x.ColumnFilters, filters)
            .Add(x => x.ApplyFiltersAndRefreshAsync, () => Task.CompletedTask)
            .Add(x => x.ClearFiltersAsync, () => { called = true; return Task.CompletedTask; })
        );

        cut.Find("fluent-button[aria-label='Limpiar filtros']").Click();

        called.Should().BeTrue();
    }
}


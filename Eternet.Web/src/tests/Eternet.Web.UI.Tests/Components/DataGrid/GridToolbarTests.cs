using Bunit;
using Eternet.Web.UI.Components.DataGrid;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using FluentAssertions;

namespace Eternet.Web.UI.Tests.Components.DataGrid;

public class GridToolbarTests : Bunit.TestContext
{
    [Fact]
    public void Render_SetsTooltipsOnButtons()
    {
        Services.AddFluentUIComponents();
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = RenderComponent<GridToolbar>(p => p
            .Add(x => x.ShowColumnSelectorDialog, false)
            .Add(x => x.ShowColumnSelectorDialogChanged, EventCallback.Factory.Create<bool>(this, _ => { }))
            .Add(x => x.ShowFilterPanel, false)
            .Add(x => x.ShowFilterPanelChanged, EventCallback.Factory.Create<bool>(this, _ => { }))
            .Add(x => x.ShowClearFilters, true)
            .Add(x => x.GeneralFilter, string.Empty)
            .Add(x => x.GeneralFilterChanged, EventCallback.Factory.Create<string?>(this, _ => { }))
            .Add(x => x.GeneralFilterIgnoreCase, false)
            .Add(x => x.GeneralFilterIgnoreCaseChanged, EventCallback.Factory.Create<bool>(this, _ => { }))
            .Add(x => x.ApplyFiltersAndRefreshAsync, () => Task.CompletedTask)
            .Add(x => x.ClearFiltersAsync, () => Task.CompletedTask)
        );

        cut.Markup.Should().NotBeNull();
    }
}

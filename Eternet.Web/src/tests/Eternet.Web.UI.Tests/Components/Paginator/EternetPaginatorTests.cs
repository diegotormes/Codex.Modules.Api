using Bunit;
using FluentAssertions;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Eternet.Web.UI.Tests.Components.Paginator;

public class EternetPaginatorTests : Bunit.TestContext
{
    [Fact]
    public async Task HandleItemsPerPageChange_NewValue_ResetsCurrentPageIndex()
    {
        var state = new PaginationState { ItemsPerPage = 10 };
        await state.SetCurrentPageIndexAsync(2);
        await state.SetTotalItemCountAsync(100);
        Services.AddFluentUIComponents();
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = RenderComponent<EternetPaginator>(p => p.Add(x => x.State, state));
        cut.Find("fluent-select").Change("25");

        state.ItemsPerPage.Should().Be(25);
        state.CurrentPageIndex.Should().Be(0);
    }

    [Fact]
    public async Task HandleItemsPerPageChange_SameValue_DoesNotChangeState()
    {
        var state = new PaginationState { ItemsPerPage = 10 };
        await state.SetCurrentPageIndexAsync(1);
        await state.SetTotalItemCountAsync(100);
        Services.AddFluentUIComponents();
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = RenderComponent<EternetPaginator>(p => p.Add(x => x.State, state));
        cut.Find("fluent-select").Change("10");

        state.ItemsPerPage.Should().Be(10);
        state.CurrentPageIndex.Should().Be(1);
    }
}

using Eternet.Web.UI.Abstractions;
using Eternet.Web.UI.Abstractions.DataGrid;
using Invoices.Incoming.WebApi.Components.DataGrid;
using FluentAssertions;

namespace Eternet.Web.UI.Tests.Services;

public class GridColumnServiceTests
{
    private class Sample
    {
        [EternetGridColumn(title: "Id", allowsSorting: true)]
        public int Id { get; set; }

        [EternetGridColumn(autoRender: false)]
        public string? Ignored { get; set; }

        [EternetGridColumn(allowsFiltering: false)]
        [Filterable]
        public string? Filtered { get; set; }
    }

    [Fact]
    public void GetColumnsFor_BuildsMetadataAndCaches()
    {
        var service = new GridColumnService();

        var first = service.GetColumnsFor<Sample>();
        var second = service.GetColumnsFor<Sample>();

        first.Should().BeSameAs(second);
        first.Keys.Should().Contain([nameof(Sample.Id), nameof(Sample.Filtered)]);
        first[nameof(Sample.Filtered)].AllowsFiltering.Should().BeTrue();
    }
}

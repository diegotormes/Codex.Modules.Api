using Eternet.Accounting.Api.Features.VatClosures.Preview;
using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews;
using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;
using Microsoft.EntityFrameworkCore;

namespace Eternet.Accounting.Api.Tests.Features.Steps;

public class ClearVatRetentionViewAsyncTests
{
    [Fact]
    public async Task Handle_RemovesEntriesForMonth()
    {
        var options = new DbContextOptionsBuilder<ViewsContext>()
            .UseInMemoryDatabase("retention-clear")
            .Options;
        await using var context = new ViewsContext(options);
        context.VatRetentions.AddRange(
            new VatRetentionEntry { Id = 1, Date = new DateOnly(2024, 5, 5) },
            new VatRetentionEntry { Id = 2, Date = new DateOnly(2024, 6, 5) });
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var step = new ClearVatRetentionViewAsync(context);
        await step.Handle(new RefreshVatRetentionViewHandler.Request { Month = 5, Year = 2024 }, CancellationToken.None);

        context.VatRetentions.Should().ContainSingle();
        context.VatRetentions.Single().Id.Should().Be(2);
    }
}

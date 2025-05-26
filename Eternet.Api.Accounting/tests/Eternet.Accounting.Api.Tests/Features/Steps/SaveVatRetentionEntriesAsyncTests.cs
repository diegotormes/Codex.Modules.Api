using Eternet.Accounting.Api.Features.VatClosures.Preview;
using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews;
using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;
using Microsoft.EntityFrameworkCore;

namespace Eternet.Accounting.Api.Tests.Features.Steps;

public class SaveVatRetentionEntriesAsyncTests
{
    [Fact]
    public async Task Handle_SavesEntries()
    {
        var options = new DbContextOptionsBuilder<ViewsContext>()
            .UseInMemoryDatabase("retention-save")
            .Options;
        await using var context = new ViewsContext(options);
        var sourceOptions = new DbContextOptionsBuilder<ViewsContext>()
            .UseInMemoryDatabase("retention-source")
            .Options;
        await using var source = new ViewsContext(sourceOptions);
        source.VatRetentions.AddRange(
            new VatRetentionEntry { Id = 1, Date = new DateOnly(2024,5,1) },
            new VatRetentionEntry { Id = 2, Date = new DateOnly(2024,5,2) });
        await source.SaveChangesAsync(TestContext.Current.CancellationToken);
        var sales = source.VatRetentions.Where(e => e.Id == 1);
        var purchases = source.VatRetentions.Where(e => e.Id == 2);
        var step = new SaveVatRetentionEntriesAsync(context);

        await step.Handle(sales, purchases, CancellationToken.None);

        context.VatRetentions.Should().HaveCount(2);
    }
}

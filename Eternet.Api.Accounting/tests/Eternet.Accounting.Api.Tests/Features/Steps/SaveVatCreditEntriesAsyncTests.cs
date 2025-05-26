using Eternet.Accounting.Api.Features.VatClosures.Preview;
using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews;
using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;
using Microsoft.EntityFrameworkCore;

namespace Eternet.Accounting.Api.Tests.Features.Steps;

public class SaveVatCreditEntriesAsyncTests
{
    [Fact]
    public async Task Handle_SavesEntries()
    {
        var options = new DbContextOptionsBuilder<ViewsContext>()
            .UseInMemoryDatabase("credit-save")
            .Options;
        await using var context = new ViewsContext(options);
        var sourceOptions = new DbContextOptionsBuilder<ViewsContext>()
            .UseInMemoryDatabase("credit-source")
            .Options;
        await using var source = new ViewsContext(sourceOptions);
        source.VatCredits.Add(new VatCreditEntry { Id = 1, VatRate = 21 });
        await source.SaveChangesAsync(TestContext.Current.CancellationToken);
        var entries = source.VatCredits.AsQueryable();
        var step = new SaveVatCreditEntriesAsync(context);

        await step.Handle(entries, CancellationToken.None);

        context.VatCredits.Should().ContainSingle();
    }
}

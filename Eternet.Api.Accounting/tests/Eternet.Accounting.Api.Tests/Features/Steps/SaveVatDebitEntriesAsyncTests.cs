using Eternet.Accounting.Api.Features.VatClosures.Preview;
using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews;
using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;
using Microsoft.EntityFrameworkCore;

namespace Eternet.Accounting.Api.Tests.Features.Steps;

public class SaveVatDebitEntriesAsyncTests
{
    [Fact]
    public async Task Handle_SavesEntries()
    {
        var options = new DbContextOptionsBuilder<ViewsContext>()
            .UseInMemoryDatabase("debit-save")
            .Options;
        await using var context = new ViewsContext(options);
        var sourceOptions = new DbContextOptionsBuilder<ViewsContext>()
            .UseInMemoryDatabase("debit-source")
            .Options;
        await using var source = new ViewsContext(sourceOptions);
        source.VatDebits.Add(new VatDebitEntry { Id = 1, VatRate = 21 });
        await source.SaveChangesAsync(TestContext.Current.CancellationToken);
        var entries = source.VatDebits.AsQueryable();
        var step = new SaveVatDebitEntriesAsync(context);

        await step.Handle(entries, CancellationToken.None);

        context.VatDebits.Should().ContainSingle();
    }
}

using Eternet.Accounting.Api.Features.VatClosures.Preview;
using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews;
using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;
using Microsoft.EntityFrameworkCore;

namespace Eternet.Accounting.Api.Tests.Features.Steps;

public class ClearVatDebitViewAsyncTests
{
    [Fact]
    public async Task Handle_RemovesAllEntries()
    {
        var options = new DbContextOptionsBuilder<ViewsContext>()
            .UseInMemoryDatabase("debit-clear")
            .Options;
        await using var context = new ViewsContext(options);
        context.VatDebits.AddRange(
            new VatDebitEntry { Id = 1, VatRate = 21 },
            new VatDebitEntry { Id = 2, VatRate = 10 });
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var step = new ClearVatDebitViewAsync(context);
        await step.Handle(new RefreshVatDebitViewHandler.Request { Month = 5, Year = 2024 }, CancellationToken.None);

        context.VatDebits.Should().BeEmpty();
    }
}

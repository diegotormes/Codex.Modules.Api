using Eternet.Accounting.Api.Features.VatClosures.Preview;
using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews;
using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;
using Microsoft.EntityFrameworkCore;

namespace Eternet.Accounting.Api.Tests.Features.Steps;

public class ClearVatCreditViewAsyncTests
{
    [Fact]
    public async Task Handle_RemovesAllEntries()
    {
        var options = new DbContextOptionsBuilder<ViewsContext>()
            .UseInMemoryDatabase("credit-clear")
            .Options;
        await using var context = new ViewsContext(options);
        context.VatCredits.AddRange(
            new VatCreditEntry { Id = 1, VatRate = 21 },
            new VatCreditEntry { Id = 2, VatRate = 10 });
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var step = new ClearVatCreditViewAsync(context);
        await step.Handle(new RefreshVatCreditViewHandler.Request { Month = 5, Year = 2024 }, CancellationToken.None);

        context.VatCredits.Should().BeEmpty();
    }
}

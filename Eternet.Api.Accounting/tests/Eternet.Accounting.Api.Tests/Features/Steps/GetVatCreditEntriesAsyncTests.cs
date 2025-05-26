using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;
using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews;
using Eternet.Accounting.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Eternet.Accounting.Api.Tests.Features.Steps;

public class GetVatCreditEntriesAsyncTests
{
    [Fact]
    public async Task Handle_ReturnsEntries()
    {
        var options = new DbContextOptionsBuilder<AccountingContext>()
            .UseInMemoryDatabase("credit-get")
            .Options;
        await using var context = new AccountingContext(options);
        context.VatPurchases.Add(new VatPurchase
        {
            Id = 1,
            VatDate = new DateOnly(2024, 5, 15),
            VatRate = 21,
            TaxableAmount = 100,
            VatAmount = 21,
            Total = 121
        });
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var step = new GetVatCreditEntries(context);
        var query = step.Handle(new RefreshVatCreditViewHandler.Request { Month = 5, Year = 2024 });
        var list = await query.AsNoTracking().ToListAsync(cancellationToken: TestContext.Current.CancellationToken);

        list.Should().ContainSingle();
        list[0].VatRate.Should().Be(21);
    }
}

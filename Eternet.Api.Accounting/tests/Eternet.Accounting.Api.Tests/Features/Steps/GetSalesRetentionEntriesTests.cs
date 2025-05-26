using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;
using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews;
using Eternet.Accounting.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Eternet.Accounting.Api.Tests.Features.Steps;

public class GetSalesRetentionEntriesTests
{
    [Fact]
    public async Task Handle_ReturnsEntries()
    {
        var options = new DbContextOptionsBuilder<AccountingContext>()
            .UseInMemoryDatabase("sales-ret")
            .Options;
        await using var context = new AccountingContext(options);
        context.RetentionTypes.Add(new RetentionType { Id = 1, Description = "IVA" });
        context.Customers.Add(new Customer { Id = 1, Name = "c", TaxNumber = "1" });
        context.CustomerAccounts.Add(new CustomerAccount { Id = 1, ClientId = 1 });
        context.SalesRetentions.Add(new RetentionSale
        {
            Id = 1,
            MovementId = 1,
            RetentionTypeId = 1,
            Date = new DateOnly(2024,5,5),
            Amount = 10,
            RetentionCode = "R",
            Month = 5,
            Year = 2024
        });
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var step = new GetSalesRetentionEntries(context);
        var query = step.Handle(new RefreshVatRetentionViewHandler.Request { Month = 5, Year = 2024 });
        var list = await query.AsNoTracking().ToListAsync(cancellationToken: TestContext.Current.CancellationToken);

        list.Should().ContainSingle();
        list[0].Amount.Should().Be(10);
    }
}

using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;
using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews;
using Eternet.Accounting.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Eternet.Accounting.Api.Tests.Features.Steps;

public class GetPurchasesRetentionEntriesTests
{
    [Fact]
    public async Task Handle_ReturnsEntries()
    {
        var options = new DbContextOptionsBuilder<AccountingContext>()
            .UseInMemoryDatabase("purchases-ret")
            .Options;
        await using var context = new AccountingContext(options);
        context.RetentionTypes.Add(new RetentionType { Id = 1, Description = "IVA" });
        context.VatPurchases.Add(new VatPurchase { Id = 1, InvoiceDate = new DateOnly(2024,5,1), VatDate = new DateOnly(2024,5,1), Provider = "p", Cuit = "1", VatRate = 21, TaxableAmount = 100, VatAmount = 21, Total = 121 });
        context.PurchaseRetentions.Add(new RetentionPurchase
        {
            Id = 1,
            VatPurchaseId = 1,
            RetentionCode = "R",
            Month = 5,
            Year = 2024,
            RetentionTypeId = 1
        });
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var step = new GetPurchasesRetentionEntries(context);
        var query = step.Handle(new RefreshVatRetentionViewHandler.Request { Month = 5, Year = 2024 });
        var list = await query.AsNoTracking().ToListAsync(cancellationToken: TestContext.Current.CancellationToken);

        list.Should().ContainSingle();
        list[0].Name.Should().Be("p");
    }
}

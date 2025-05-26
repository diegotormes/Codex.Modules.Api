using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;
using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews;
using Eternet.Accounting.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Eternet.Accounting.Api.Tests.Features.Steps;

public class GetVatDebitEntriesAsyncTests
{
    [Fact]
    public async Task Handle_ReturnsEntries()
    {
        var options = new DbContextOptionsBuilder<AccountingContext>()
            .UseInMemoryDatabase("debit-get")
            .Options;
        await using var context = new AccountingContext(options);
        var invoice = new SalesInvoice
        {
            Id = 1,
            VoucherType = 1,
            PointOfSale = 1,
            Date = new DateOnly(2024, 5, 10),
            TaxResponsibility = "RI"
        };
        context.SalesInvoices.Add(invoice);
        context.SalesInvoiceDetails.Add(new SalesInvoiceDetail
        {
            Id = 1,
            InvoiceId = 1,
            Quantity = 2,
            Price = 50,
            VatRate = 21,
            Vat = 21
        });
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var step = new GetVatDebitEntries(context);
        var query = step.Handle(new RefreshVatDebitViewHandler.Request { Month = 5, Year = 2024 });
        var list = await query.AsNoTracking().ToListAsync(cancellationToken: TestContext.Current.CancellationToken);

        list.Should().ContainSingle();
        list[0].VatAmount.Should().BeApproximately(21, 0.01m);
    }
}

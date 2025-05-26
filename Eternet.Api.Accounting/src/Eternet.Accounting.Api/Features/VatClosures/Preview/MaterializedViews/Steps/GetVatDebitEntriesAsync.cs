namespace Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;

public class GetVatDebitEntries(AccountingContext dbContext) : BaseHandlerResponses
{
    public IQueryable<VatDebitEntry> Handle(RefreshVatDebitViewHandler.Request request)
    {
        var start = new DateOnly(request.Year, request.Month, 1);
        var end = start.AddMonths(1);
        var january2013 = new DateOnly(2013, 1, 1);

        return dbContext.SalesInvoiceDetails
            .Join(dbContext.SalesInvoices,
                d => d.InvoiceId,
                f => f.Id,
                (d, f) => new { f, d })
            .Where(fd =>
                fd.f.PointOfSale <= 50 &&
                (fd.f.VoucherType == 1 || fd.f.VoucherType == 3) &&
                fd.f.Date >= start && fd.f.Date < end &&
                (fd.f.PointOfSale != 11 || fd.f.Date >= january2013))
            .GroupBy(fd => new { fd.f.TaxResponsibility, fd.d.VatRate })
            .Select(g => new
            {
                g.Key.TaxResponsibility,
                g.Key.VatRate,
                Taxable = g.Sum(x => x.d.Quantity * x.d.Price),
                Total = g.Sum(x => x.d.Vat + x.d.Quantity * x.d.Price)
            })
            .Where(x => x.Taxable >= 0.01m)
            .Select(x => new VatDebitEntry
            {
                Id = 1,
                Description = "Debito Fiscal del Periodo",
                TaxResponsibility = x.TaxResponsibility,
                VatRate = x.VatRate,
                TaxableAmount = x.Taxable,
                VatAmount = Math.Round(
                    x.TaxResponsibility == "RI"
                        ? x.Taxable * (x.VatRate / 100m)
                        : (x.VatRate / 100m) * x.Total / (1 + x.VatRate / 100m),
                    2, MidpointRounding.AwayFromZero),
                Total = x.Total
            });
    }
}

namespace Eternet.Accounting.Api.Features.VatClosures;

public static class VatQueries
{
    const string VatDebitDescription = "Debito Fiscal del Periodo";
    const string VatCreditDescription = "Credito Fiscal del Periodo";

    public static IQueryable<VatDebitEntry> BuildVatDebitsQuery(
        this AccountingContext dbContext,
        short month,
        short year)
    {
        var start = new DateOnly(year, month, 1);
        var end = start.AddMonths(1);
        var january2013 = new DateOnly(2013, 1, 1);

        return dbContext.SalesInvoiceDetails
            .Join(dbContext.SalesInvoices,
                d => d.InvoiceId,
                f => f.Id,
                (d, f) => new { f, d })
            .Where(fd => fd.f.PointOfSale <= 50 &&
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
                Description = VatDebitDescription,
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

    public static IQueryable<VatCreditEntry> BuildVatCreditsQuery(
        this AccountingContext dbContext,
        short month,
        short year)
    {
        var start = new DateOnly(year, month, 1);
        var end = start.AddMonths(1);

        return dbContext.VatPurchases
            .Where(p => p.VatDate >= start && p.VatDate < end)
            .GroupBy(p => p.VatRate)
            .Select(g => new
            {
                VatRate = g.Key,
                TaxableAmount = g.Sum(x => x.TaxableAmount),
                VatAmount = g.Sum(x => x.VatAmount),
                Total = g.Sum(x => x.Total)
            })
            .Where(x => x.VatAmount >= 0.01m)
            .Select(x => new VatCreditEntry
            {
                Id = 1,
                Description = VatCreditDescription,
                VatRate = x.VatRate,
                TaxableAmount = x.TaxableAmount,
                VatAmount = x.VatAmount,
                Total = x.Total
            });
    }
}

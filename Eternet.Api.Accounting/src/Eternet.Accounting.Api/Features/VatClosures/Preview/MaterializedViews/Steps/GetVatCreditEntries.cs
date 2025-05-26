namespace Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;

public class GetVatCreditEntries(AccountingContext dbContext) : BaseHandlerResponses
{
    public IQueryable<VatCreditEntry> Handle(RefreshVatCreditViewHandler.Request request)
    {
        var start = new DateOnly(request.Year, request.Month, 1);
        var end = start.AddMonths(1);

        return dbContext.VatPurchases
            .Where(p => p.VatDate >= start && p.VatDate < end)
            .GroupBy(p => p.VatRate)
            .Select(g => new VatCreditEntry
            {
                Id = 1,
                Description = "Credito Fiscal del Periodo",
                VatRate = g.Key,
                TaxableAmount = g.Sum(x => x.TaxableAmount),
                VatAmount = g.Sum(x => x.VatAmount),
                Total = g.Sum(x => x.Total)
            })
            .Where(x => x.VatAmount >= 0.01m);
    }
}

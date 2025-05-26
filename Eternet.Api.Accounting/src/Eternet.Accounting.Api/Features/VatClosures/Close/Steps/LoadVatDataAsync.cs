using Eternet.Accounting.Contracts.VatClosures;

namespace Eternet.Accounting.Api.Features.VatClosures.Close.Steps;

public record VatClosesData(
    decimal Debit,
    decimal Credit,
    decimal Retention,
    decimal SocialSecurity);

public class LoadVatDataAsync(AccountingContext dbContext) : BaseHandlerResponses
{
    public async Task<VatClosesData> Handle(
        CloseVat.Request request,
        List<VatSalaryItem> salaries,
        CancellationToken cancellationToken)
    {
        var debits = await dbContext.BuildVatDebitsQuery(request.Month, request.Year)
            .Select(d => new VatDebitItem { VatAmount = d.VatAmount })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var credits = await dbContext.BuildVatCreditsQuery(request.Month, request.Year)
            .Select(c => new VatCreditItem { VatAmount = c.VatAmount })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var rets = await dbContext.BuildRetentionsQuery(request.Month, request.Year)
            .Select(r => new VatRetentionItem { RetentionAmount = r.Amount })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var totals = new VatClosesData(
            debits.Sum(x => x.VatAmount ?? 0),
            credits.Sum(x => x.VatAmount ?? 0),
            rets.Sum(x => x.RetentionAmount ?? 0),
            salaries.Sum(x => x.FinalConsumerAmount ?? 0));

        return totals;
    }
}

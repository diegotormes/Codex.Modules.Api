using Eternet.Accounting.Contracts.VatClosures;

namespace Eternet.Accounting.Api.Features.VatClosures.Close.Steps;

public class LoadSalariesAsync(AccountingContext dbContext) : BaseHandlerResponses
{
    public async Task<List<VatSalaryItem>> Handle(
        CloseVat.Request request,
        CancellationToken cancellationToken)
    {
        var rate = await dbContext.SalaryItems
            .Where(i => i.Id == 138)
            .Select(i => i.Value * 0.01m)
            .FirstAsync(cancellationToken);

        var salaries = await dbContext.SalaryClosureDetails
            .Join(dbContext.SalaryClosures,
                detail => detail.ClosureId,
                closure => closure.Id,
                (detail, closure) => new { detail, closure })
            .Where(x =>
                x.detail.AccountId == 1064 &&
                x.closure.Month <= 12 &&
                !dbContext.VatSalaryClosureDetails.Any(v => v.SalaryClosureDetailId == x.detail.Id))
            .Select(x => new VatSalaryItem
            {
                Id = x.detail.Id,
                TotalAmount = x.closure.BaseCalculationCfIva != null && x.closure.BaseCalculationCfIva != 0
                    ? x.closure.BaseCalculationCfIva.Value
                    : x.detail.Amount,
                FinalConsumerAmount = (x.closure.BaseCalculationCfIva != null && x.closure.BaseCalculationCfIva != 0
                    ? x.closure.BaseCalculationCfIva.Value
                    : x.detail.Amount) * rate
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return salaries;
    }
}

using Eternet.Accounting.Contracts.VatClosures;

namespace Eternet.Accounting.Api.Features.VatClosures.Close.Steps;

public class SaveSalaryDetailsAsync(AccountingContext dbContext) : BaseHandlerResponses
{
    public async Task<StepResult> Handle(
        CloseVat.Request request,
        List<VatSalaryItem> salaryItems,
        CancellationToken cancellationToken)
    {
        foreach (var item in salaryItems)
        {
            var entity = new VatSalaryClosureDetail
            {
                SalaryClosureDetailId = item.Id,
                Month = request.Month,
                Year = request.Year,
                Total = item.TotalAmount,
                IvaCf = item.FinalConsumerAmount
            };
            dbContext.VatSalaryClosureDetails.Add(entity);
        }
        if (salaryItems.Count > 0)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        return Next(cancellationToken);
    }
}

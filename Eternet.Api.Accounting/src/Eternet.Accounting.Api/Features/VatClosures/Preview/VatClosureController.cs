using Eternet.Accounting.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Eternet.Accounting.Api.Features.VatClosures.Preview;

[Route(Config.ControllerName)]
public class VatClosureController(AccountingContext dbContext, ViewsContext viewContext, IMediator mediator) : ODataController
{
    [EnableQuery(PageSize = 100)]
    [HttpGet(Config.VatDebits)]
    public async Task<IQueryable<VatDebitResponse>> GetVatDebits(short month, short year)
    {
        await mediator.Send(new RefreshVatDebitViewHandler.Request { Month = month, Year = year });
        return viewContext.VatDebits.AsNoTracking()
            .Select(VatClosureMappingExpressions.ToVatDebitResponseExpression());
    }

    [EnableQuery(PageSize = 100)]
    [HttpGet(Config.VatCredits)]
    public async Task<IQueryable<VatCreditResponse>> GetVatCredits(short month, short year)
    {
        await mediator.Send(new RefreshVatCreditViewHandler.Request { Month = month, Year = year });
        return viewContext.VatCredits.AsNoTracking()
            .Select(VatClosureMappingExpressions.ToVatCreditResponseExpression());
    }

    [EnableQuery(PageSize = 100)]
    [HttpGet(Config.VatRetentions)]
    public async Task<IQueryable<VatRetentionResponse>> GetVatRetentions(short month, short year)
    {
        await mediator.Send(new RefreshVatRetentionViewHandler.Request { Month = month, Year = year });
        return viewContext.VatRetentions.AsNoTracking()
            .Select(VatClosureMappingExpressions.ToVatRetentionResponseExpression());
    }

    [EnableQuery(PageSize = 100)]
    [HttpGet(Config.VatSalaries)]
    public IQueryable<VatSalaryResponse> GetVatSalaries()
    {
        var rate = dbContext.SalaryItems
            .Where(i => i.Id == 138)
            .Select(i => i.Value * 0.01m)
            .First();

        return dbContext.SalaryClosureDetails
            .Where(d => d.AccountId == 1064
                && d.Closure.Month <= 12
                && !dbContext.VatSalaryClosureDetails.Any(v => v.SalaryClosureDetailId == d.Id))
            .Select(d => new VatSalaryEntry
            {
                Month = d.Closure.Month,
                Year = d.Closure.Year,
                Id = d.Id,
                Description = d.Description,
                Total = d.Closure.BaseCalculationCfIva != null && d.Closure.BaseCalculationCfIva != 0
                    ? d.Closure.BaseCalculationCfIva.Value
                    : d.Amount,
                IvaCf = (d.Closure.BaseCalculationCfIva != null && d.Closure.BaseCalculationCfIva != 0
                    ? d.Closure.BaseCalculationCfIva.Value
                    : d.Amount) * rate
            })
            .AsNoTracking()
            .Select(VatClosureMappingExpressions.ToVatSalaryResponseExpression());
    }
}

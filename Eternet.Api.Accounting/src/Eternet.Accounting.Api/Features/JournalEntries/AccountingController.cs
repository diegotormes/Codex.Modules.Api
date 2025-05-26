using Eternet.Accounting.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Eternet.Accounting.Api.Features.JournalEntries;

[Route(Config.ControllerName)]
public class AccountingController(AccountingContext dbContext) : ODataController
{
    [EnableQuery(PageSize = 100)]
    [HttpGet(Config.JournalEntries)]
    public IQueryable<JournalEntryResponse> GetJournalEntries()
    {
        return dbContext.JournalEntries.Select(
            JournalEntryMappingExpressions.ToJournalEntryResponseExpression());
    }

    [EnableQuery]
    [HttpGet($"{Config.JournalEntries}/{{id}}")]
    public SingleResult<JournalEntryResponse> GetInvoice(Guid id)
    {
        return SingleResult.Create(dbContext.JournalEntries.Where(i => i.Guid == id).Select(
            JournalEntryMappingExpressions.ToJournalEntryResponseExpression()));
    }

    [EnableQuery(PageSize = 100)]
    [HttpGet(Config.GeneralLedgerAccounts)]
    public IQueryable<GeneralLedgerAccountResponse> GetGeneralLedgerAccounts()
    {
        return dbContext.GeneralLedgerAccounts.Select(
            GeneralLedgerAccountMappingExpressions.ToGeneralLedgerAccountResponseExpression());
    }

    [EnableQuery]
    [HttpGet($"{Config.GeneralLedgerAccounts}/{{id}}")]
    public SingleResult<GeneralLedgerAccountResponse> GetGeneralLedgerAccountById(int id)
    {
        return SingleResult.Create(
            dbContext.GeneralLedgerAccounts
                .Where(a => a.Id == id)
                .Select(GeneralLedgerAccountMappingExpressions.ToGeneralLedgerAccountResponseExpression())
        );
    }

}
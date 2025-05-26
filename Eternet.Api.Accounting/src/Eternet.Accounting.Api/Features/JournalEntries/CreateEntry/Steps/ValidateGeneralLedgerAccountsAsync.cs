namespace Eternet.Accounting.Api.Features.JournalEntries.CreateEntry.Steps;

//Just a sample review logic with Administration team
public class ValidateGeneralLedgerAccountsAsync(AccountingContext dbContext) : BaseHandlerResponses
{
    public async Task<StepResult> Handle(
        CreateEntryAccountingJournal.Request request,
        CancellationToken cancellationToken)
    {
        var requestedAccountIds = request.JournalDetails
            .Select(jd => jd.GeneralLedgerAccountId)
            .Distinct()
            .ToList();

        var existingAccountIds = await dbContext.GeneralLedgerAccounts
            .Where(acc =>
                //Pending = null, Current = -2
                (acc.AccountingPeriodId == null || acc.AccountingPeriodId == -2) && 
                requestedAccountIds.Contains(acc.Id))
            .Select(acc => acc.Id)
            .ToListAsync(cancellationToken);

        var missingAccountIds = requestedAccountIds.Except(existingAccountIds).ToList();

        if (missingAccountIds.Count != 0)
        {
            var missingIdsString = string.Join(", ", missingAccountIds);
            return ValidationError($"The following GeneralLedgerAccountIds do not exist or are not valid for the accounting period: '{request.PeriodStatus}'");
        }

        return Next(cancellationToken);
    }
}
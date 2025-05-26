namespace Eternet.Accounting.Api.Features.JournalEntries.Shared;

//Just a sample review logic with Administration team
public class CheckActiveAccountingPeriodAsync(AccountingContext dbContext) : BaseHandlerResponses
{
    public async Task<StepResult<AccountingPeriod>> Handle(
        DateOnly date,
        JournalEntryPeriodStatus periodStatus, 
        CancellationToken cancellationToken)
    {
        AccountingPeriod? accountingPeriod;
        if (periodStatus == JournalEntryPeriodStatus.Pending)
        {
            accountingPeriod = await dbContext.AccountingPeriods
                .Where(p => date >= p.StartDate && date <= p.EndDate && p.CloseDate == null)
                .OrderByDescending(p => p.EndDate)
                .FirstOrDefaultAsync(cancellationToken);
        }
        else if (periodStatus == JournalEntryPeriodStatus.Current)
        {
            accountingPeriod = await dbContext.AccountingPeriods
                .Where(p => date >= p.StartDate && date <= p.EndDate)
                .OrderByDescending(p => p.EndDate)
                .FirstOrDefaultAsync(cancellationToken);
        }
        else
        {
            return InvalidStateError($"Accounting: Invalid period status {periodStatus}.");
        }

        if (accountingPeriod is null)
        {
            return InvalidStateError($"Accounting: No active and open period found for date {date}.");
        }

        if (accountingPeriod.PartialCloseDate.HasValue && date > accountingPeriod.PartialCloseDate)
        {
             return InvalidStateError($"Accounting period '{accountingPeriod.Description}' is partially closed after {accountingPeriod.PartialCloseDate}. Cannot create entries after this date.");
        }

        return accountingPeriod;
    }
}

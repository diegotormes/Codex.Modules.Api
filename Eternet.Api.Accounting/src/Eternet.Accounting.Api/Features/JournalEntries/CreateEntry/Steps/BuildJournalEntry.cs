namespace Eternet.Accounting.Api.Features.JournalEntries.CreateEntry.Steps;

public class BuildJournalEntry
{
    public JournalEntry Handle(CreateEntryAccountingJournal.Request request)
    {
        var newEntry = new JournalEntry
        {
            Guid = Guid.CreateVersion7(),
            AccountingPeriodId = GetAccountingPeriodId(request.PeriodStatus),
            Date = request.Date,
            Description = request.Description,
            Month = (short)request.Date.Month,
            Year = (short)request.Date.Year,
            EntryClose = 0, // Check with the Administration team if this is correct
            AccountingEntryDetails = request.JournalDetails
            .Select(jd => new JournalEntryDetail
            {
                GeneralLedgerAccountId = jd.GeneralLedgerAccountId,
                Debit = jd.Debit,
                Credit = jd.Credit
            }).ToList()
        };

        return newEntry;
    }

    private static int? GetAccountingPeriodId(JournalEntryPeriodStatus periodStatus)
    {
        return periodStatus switch
        {
            JournalEntryPeriodStatus.Pending => null,
            JournalEntryPeriodStatus.Current => -2,
            _ => throw new ArgumentOutOfRangeException(nameof(periodStatus), periodStatus, null)
        };
    }
}

public class CheckPeriodStatusIsValid : BaseHandlerResponses
{
    public StepResult Handle(AccountingPeriod accountingPeriod, DateOnly date)
    {
        if (accountingPeriod.PartialCloseDate.HasValue && date > accountingPeriod.PartialCloseDate)
        {
            return InvalidStateError($"Accounting period '{accountingPeriod.Description}' is partially closed after {accountingPeriod.PartialCloseDate}. Cannot create entries after this date.");
        }
        return Next();

    }
}

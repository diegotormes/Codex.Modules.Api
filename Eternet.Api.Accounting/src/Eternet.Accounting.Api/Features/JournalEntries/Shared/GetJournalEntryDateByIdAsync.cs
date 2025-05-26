namespace Eternet.Accounting.Api.Features.JournalEntries.Shared;

public class GetJournalEntryDateByIdAsync(AccountingContext dbContext) : BaseHandlerResponses
{
    public async Task<StepResult<DateOnly>> Handle(
        Guid id,
        CancellationToken cancellationToken)
    {
        var journalEntry = await dbContext.JournalEntries
            .FirstOrDefaultAsync(je => je.Guid == id, cancellationToken);
        if (journalEntry is null)
        {
            return NotFoundError($"Journal entry with ID {id} not found.");
        }
        return journalEntry.Date;
    }
}
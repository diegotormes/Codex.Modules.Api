using Eternet.Accounting.Api.Features.JournalEntries.CreateEntry.Steps;
using Eternet.Accounting.Api.Features.JournalEntries.Shared;

namespace Eternet.Accounting.Api.Features.JournalEntries.CreateEntry;

[HandlerForEntity<JournalEntry>]
[GenerateExecutePipeline<CheckActiveAccountingPeriodAsync>]
//[GenerateExecutePipeline<CheckPeriodStatusIsValid>]
[GenerateExecutePipeline<ValidateGeneralLedgerAccountsAsync>]
[GenerateExecutePipeline<BuildJournalEntry>]
public class CreateEntryAccountingJournalHandler(ScopedStates scopedStates) : CreateEntryAccountingJournal
{
    public override Response Handle(Request request)
    {
        var journalEntry = scopedStates.GetRequiredState<JournalEntry>();

        var response = new Response
        {
            Id = journalEntry.Guid,
            Entity = journalEntry
        };

        return response;
    }
}
using Eternet.Accounting.Api.Features.JournalEntries.Shared;

namespace Eternet.Accounting.Api.Features.JournalEntries.DeleteEntry;

[HandlerForEntity<JournalEntry>]
[GenerateExecutePipeline<GetJournalEntryDateByIdAsync>]
//[GenerateExecutePipeline<CheckActiveAccountingPeriodAsync>]
public class DeleteEntryAccountingJournalHandler : DeleteEntryAccountingJournal
{
    public override Response Handle(Request request)
    {
        var response = new Response
        {
            Id = request.Id
        };
        return response;
    }
}
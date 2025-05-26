using Eternet.Accounting.Api.Features.JournalEntries.DeleteEntry;
using Eternet.Accounting.Contracts.JournalEntries;

namespace Eternet.Accounting.Api.Tests.Features.Journal.DeleteEntry;

public class DeleteEntryAccountingJournalHandlerTests
{
    [Fact]
    public void Handle_ReturnsId()
    {
        var handler = new DeleteEntryAccountingJournalHandler();
        var id = Guid.NewGuid();
        var request = new DeleteEntryAccountingJournal.Request { Id = id };

        var response = handler.Handle(request);

        response.Id.Should().Be(id);
    }
}

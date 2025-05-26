using Eternet.Accounting.Contracts.JournalEntries;

namespace Eternet.Accounting.Contracts.Tests.Contracts.JournalEntries;

public class DeleteEntryAccountingJournalValidatorTests
{
    private static DeleteEntryAccountingJournal.Request CreateValidRequest() => new() { Id = Guid.NewGuid() };

    [Fact]
    public void Validate_ValidRequest_Passes()
    {
        var validator = new DeleteEntryAccountingJournal.Validator();
        var request = CreateValidRequest();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_IdIsEmpty_ReturnsError()
    {
        var validator = new DeleteEntryAccountingJournal.Validator();
        var request = new DeleteEntryAccountingJournal.Request { Id = Guid.Empty };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(request.Id));
    }
}

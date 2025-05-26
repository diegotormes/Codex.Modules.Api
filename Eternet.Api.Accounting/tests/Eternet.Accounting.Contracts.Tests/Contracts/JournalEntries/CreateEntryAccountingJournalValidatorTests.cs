using Eternet.Accounting.Contracts.JournalEntries;
using Eternet.Accounting.Contracts.JournalEntries.Responses;

namespace Eternet.Accounting.Contracts.Tests.Contracts.JournalEntries;

public class CreateEntryAccountingJournalValidatorTests
{
    private static CreateEntryAccountingJournal.Request CreateValidRequest() => new()
    {
        Date = new DateOnly(2024, 1, 1),
        PeriodStatus = JournalEntryPeriodStatus.Current,
        Description = "Valid entry",
        JournalDetails =
        [
            new() { GeneralLedgerAccountId = 1, Debit = 50, Credit = 0 },
            new() { GeneralLedgerAccountId = 2, Debit = 0, Credit = 50 }
        ]
    };

    [Fact]
    public void Validate_ValidRequest_Passes()
    {
        var validator = new CreateEntryAccountingJournal.Validator();
        var request = CreateValidRequest();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DateIsEmpty_ReturnsError()
    {
        var validator = new CreateEntryAccountingJournal.Validator();
        var request = CreateValidRequest();
        request = request with { Date = default };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(request.Date));
    }

    [Fact]
    public void Validate_DescriptionIsEmpty_ReturnsError()
    {
        var validator = new CreateEntryAccountingJournal.Validator();
        var request = CreateValidRequest();
        request = request with { Description = string.Empty };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(request.Description));
    }

    [Fact]
    public void Validate_DescriptionIsTooLong_ReturnsError()
    {
        var validator = new CreateEntryAccountingJournal.Validator();
        var request = CreateValidRequest();
        request = request with { Description = new string('a', 101) };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(request.Description));
    }

    [Fact]
    public void Validate_JournalDetailsAreEmpty_ReturnsError()
    {
        var validator = new CreateEntryAccountingJournal.Validator();
        var request = CreateValidRequest();
        request.JournalDetails.Clear();

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(request.JournalDetails));
    }

    [Fact]
    public void Validate_DetailAccountIdIsEmpty_ReturnsError()
    {
        var validator = new CreateEntryAccountingJournal.Validator();
        var request = CreateValidRequest();
        request.JournalDetails[0] = request.JournalDetails[0] with { GeneralLedgerAccountId = 0 };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName.Contains(nameof(JournalEntryDetailResponse.GeneralLedgerAccountId)));
    }

    [Fact]
    public void Validate_DetailWithoutDebitOrCredit_ReturnsError()
    {
        var validator = new CreateEntryAccountingJournal.Validator();
        var request = CreateValidRequest();
        request.JournalDetails[0] = request.JournalDetails[0] with { Debit = 0, Credit = 0 };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DebitIsNegative_ReturnsError()
    {
        var validator = new CreateEntryAccountingJournal.Validator();
        var request = CreateValidRequest();
        request.JournalDetails[0] = request.JournalDetails[0] with { Debit = -1 };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName.Contains(nameof(JournalEntryDetailResponse.Debit)));
    }

    [Fact]
    public void Validate_CreditIsNegative_ReturnsError()
    {
        var validator = new CreateEntryAccountingJournal.Validator();
        var request = CreateValidRequest();
        request.JournalDetails[1] = request.JournalDetails[1] with { Credit = -2 };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName.Contains(nameof(JournalEntryDetailResponse.Credit)));
    }

    [Fact]
    public void Validate_DebitNotEqualCredit_ReturnsError()
    {
        var validator = new CreateEntryAccountingJournal.Validator();
        var request = CreateValidRequest();
        request.JournalDetails[1] = request.JournalDetails[1] with { Credit = 60 };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(request.JournalDetails));
    }
}

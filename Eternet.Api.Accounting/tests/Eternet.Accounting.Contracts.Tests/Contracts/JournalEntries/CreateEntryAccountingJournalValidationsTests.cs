using Eternet.Accounting.Contracts.JournalEntries.Responses;
using Eternet.Accounting.Contracts.JournalEntries.Validations;

namespace Eternet.Accounting.Contracts.Tests.Contracts.JournalEntries;

public class CreateEntryAccountingJournalValidationsTests
{
    [Theory]
    [InlineData(null, null, false)]
    [InlineData(100d, 200d, false)]
    [InlineData(100d, null, true)]
    [InlineData(null, 200d, true)]
    public void HasEitherDebitOrCredit_ReturnsExpected(double? debit, double? credit, bool expected)
    {
        var detail = new JournalEntryDetailResponse
        {
            GeneralLedgerAccountId = 1,
            Debit = debit,
            Credit = credit
        };

        var result = CreateEntryAccountingJournalValidations.HasEitherDebitOrCredit(detail);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData(-1d, false)]
    [InlineData(0d, true)]
    [InlineData(5d, true)]
    public void IsPositive_ReturnsExpected(double? value, bool expected)
    {
        var result = CreateEntryAccountingJournalValidations.IsPositive(value);

        result.Should().Be(expected);
    }

    [Fact]
    public void DebitEqualsCredit_SumsAreEqual_ReturnsTrue()
    {
        var details = new List<JournalEntryDetailResponse>
        {
            new() { GeneralLedgerAccountId = 1, Debit = 50, Credit = 0 },
            new() { GeneralLedgerAccountId = 2, Debit = 0, Credit = 50 }
        };

        var result = CreateEntryAccountingJournalValidations.DebitEqualsCredit(details);

        result.Should().BeTrue();
    }

    [Fact]
    public void DebitEqualsCredit_SumsAreDifferent_ReturnsFalse()
    {
        var details = new List<JournalEntryDetailResponse>
        {
            new() { GeneralLedgerAccountId = 1, Debit = 50, Credit = 0 },
            new() { GeneralLedgerAccountId = 2, Debit = 0, Credit = 60 }
        };

        var result = CreateEntryAccountingJournalValidations.DebitEqualsCredit(details);

        result.Should().BeFalse();
    }
}

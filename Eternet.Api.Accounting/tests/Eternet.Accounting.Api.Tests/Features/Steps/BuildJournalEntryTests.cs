using Eternet.Accounting.Api.Features.JournalEntries.CreateEntry.Steps;
using Eternet.Accounting.Contracts.JournalEntries;
using Eternet.Accounting.Contracts.JournalEntries.Responses;

namespace Eternet.Accounting.Api.Tests.Features.Steps;

public class BuildJournalEntryTests
{
    [Fact]
    public void Handle_WithCurrentPeriod_BuildsEntryCorrectly()
    {
        var step = new BuildJournalEntry();
        var request = new CreateEntryAccountingJournal.Request
        {
            Date = new DateOnly(2024, 5, 1),
            PeriodStatus = JournalEntryPeriodStatus.Current,
            Description = "desc",
            JournalDetails = [new() { GeneralLedgerAccountId = 1, Debit = 10, Credit = 0 }]
        };

        var entry = step.Handle(request);

        entry.AccountingPeriodId.Should().Be(-2);
        entry.Date.Should().Be(request.Date);
        entry.Description.Should().Be(request.Description);
        entry.Month.Should().Be((short)request.Date.Month);
        entry.Year.Should().Be((short)request.Date.Year);
        entry.EntryClose.Should().Be(0);
        entry.Guid.Should().NotBe(Guid.Empty);
        entry.AccountingEntryDetails.Should().HaveCount(1);
        var detail = entry.AccountingEntryDetails.Single();
        detail.GeneralLedgerAccountId.Should().Be(1);
        detail.Debit.Should().Be(10);
        detail.Credit.Should().Be(0);
    }

    [Fact]
    public void Handle_WithPendingPeriod_SetsNullPeriodId()
    {
        var step = new BuildJournalEntry();
        var request = new CreateEntryAccountingJournal.Request
        {
            Date = new DateOnly(2024, 6, 15),
            PeriodStatus = JournalEntryPeriodStatus.Pending,
            Description = "desc",
            JournalDetails = [new() { GeneralLedgerAccountId = 2, Debit = 0, Credit = 5 }]
        };

        var entry = step.Handle(request);

        entry.AccountingPeriodId.Should().BeNull();
    }

    [Fact]
    public void Handle_WithUnknownPeriod_Throws()
    {
        var step = new BuildJournalEntry();
        var request = new CreateEntryAccountingJournal.Request
        {
            Date = new DateOnly(2024, 1, 1),
            PeriodStatus = (JournalEntryPeriodStatus)999,
            Description = "desc",
            JournalDetails = [new() { GeneralLedgerAccountId = 1, Debit = 10, Credit = 0 }]
        };

        var act = () => step.Handle(request);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}

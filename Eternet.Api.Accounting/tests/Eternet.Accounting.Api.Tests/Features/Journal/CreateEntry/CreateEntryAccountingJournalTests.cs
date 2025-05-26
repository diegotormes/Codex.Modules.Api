using Eternet.Accounting.Api.Tests.Common;
using Eternet.Accounting.Contracts.JournalEntries;
using Eternet.Accounting.Contracts.JournalEntries.Responses;
using Eternet.Accounting.Api.Model;

namespace Eternet.Accounting.Api.Tests.Features.Journal.CreateEntry;

public class CreateEntryAccountingJournalTests()
{
    private readonly AccountingContextFixture _fixture = new();

    [Fact]
    public async ValueTask CanCreateEntryAccountingJournal()
    {
        await using var transactional = await _fixture.CreateContextAsync();
        var context = transactional.Context;
        var mediator = transactional.Mediator;
        // Arrange: seed data
        var generalLedgerAccount1 = new GeneralLedgerAccount
        {
            Id = 1001,
            AccountingPeriodId = -2,
            LedgerAccountType = LedgerAccountType.Asset,
            NormalBalance = NormalBalanceType.Debit,
            Order = 1,
            Description = "Cash Account"
        };
        var generalLedgerAccount2 = new GeneralLedgerAccount
        {
            Id = 1002,
            AccountingPeriodId = -2,
            LedgerAccountType = LedgerAccountType.LiabilityAndEquity,
            NormalBalance = NormalBalanceType.Credit,
            Order = 2,
            Description = "Bank Account"
        };
        var currentAccountingPeriod = new AccountingPeriod
        {
            Id = -2,
            Description = "Current Accounting Period",
            StartDate = new DateOnly(2024, 01, 01),
            EndDate = new DateOnly(2024, 12, 31)
        };
        context.GeneralLedgerAccounts.AddRange(generalLedgerAccount1, generalLedgerAccount2);
        context.AccountingPeriods.Add(currentAccountingPeriod);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var command = new CreateEntryAccountingJournal.Request
        {
            Date = currentAccountingPeriod.EndDate.AddDays(-1),
            PeriodStatus = JournalEntryPeriodStatus.Current,
            Description = "Test Entry",
            JournalDetails =
            [
                new() { GeneralLedgerAccountId = generalLedgerAccount1.Id, Debit = 100, Credit = 0 },
                new() { GeneralLedgerAccountId = generalLedgerAccount2.Id, Debit = 0, Credit = 100}
            ]
        };

        // Act
        var result = await mediator.Send(command, TestContext.Current.CancellationToken);
        result.IsDomainResult.Should().BeTrue(result.GetErrorMessage());
    }
}

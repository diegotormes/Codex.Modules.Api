using Eternet.Accounting.Api.Features.JournalEntries.CreateEntry.Steps;
using Eternet.Accounting.Api.Model;
using Eternet.Accounting.Api.Tests.Common;
using Eternet.Accounting.Contracts.JournalEntries;
using Eternet.Accounting.Contracts.JournalEntries.Responses;

namespace Eternet.Accounting.Api.Tests.Features.Steps;

public class ValidateGeneralLedgerAccountsAsyncTests()
{
    private readonly AccountingContextFixture _fixture = new();

    [Fact]
    public async Task Handle_WhenAllAccountsExist_ReturnsNext()
    {
        await using var transactional = await _fixture.CreateContextAsync();
        var context = transactional.Context;
        context.AccountingPeriods.Add(new AccountingPeriod
        {
            Id = -2,
            Description = "Current Period",
            StartDate = new DateOnly(2024, 1, 1),
            EndDate = new DateOnly(2024, 12, 31)
        });
        context.GeneralLedgerAccounts.AddRange(
            new GeneralLedgerAccount { Id = 1, Order = 1, Description = "acc1", NormalBalance = NormalBalanceType.Debit, LedgerAccountType = LedgerAccountType.Asset, AccountingPeriodId = -2 },
            new GeneralLedgerAccount { Id = 2, Order = 2, Description = "acc2", NormalBalance = NormalBalanceType.Credit, LedgerAccountType = LedgerAccountType.Asset, AccountingPeriodId = -2 }
        );
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var step = new ValidateGeneralLedgerAccountsAsync(context);
        var request = new CreateEntryAccountingJournal.Request
        {
            Date = new DateOnly(2024, 1, 1),
            PeriodStatus = JournalEntryPeriodStatus.Current,
            Description = "desc",
            JournalDetails = [
                new() { GeneralLedgerAccountId = 1, Debit = 10, Credit = 0 },
                new() { GeneralLedgerAccountId = 2, Debit = 0, Credit = 10 }
            ]
        };

        var result = await step.Handle(request, CancellationToken.None);

        result.IsNext.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenMissingAccounts_ReturnsValidationError()
    {
        await using var transactional = await _fixture.CreateContextAsync();
        var context = transactional.Context;
        context.AccountingPeriods.Add(new AccountingPeriod
        {
            Id = -2,
            Description = "Current Period",
            StartDate = new DateOnly(2024, 1, 1),
            EndDate = new DateOnly(2024, 12, 31)
        });
        context.GeneralLedgerAccounts.Add(
            new GeneralLedgerAccount { Id = 1, Order = 1, Description = "acc1", NormalBalance = NormalBalanceType.Debit, LedgerAccountType = LedgerAccountType.Asset, AccountingPeriodId = -2 }
        );
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var step = new ValidateGeneralLedgerAccountsAsync(context);
        var request = new CreateEntryAccountingJournal.Request
        {
            Date = new DateOnly(2024, 1, 1),
            PeriodStatus = JournalEntryPeriodStatus.Current,
            Description = "desc",
            JournalDetails = [new() { GeneralLedgerAccountId = 2, Debit = 10, Credit = 0 }]
        };

        var result = await step.Handle(request, CancellationToken.None);

        result.IsValidationError.Should().BeTrue();
    }
}

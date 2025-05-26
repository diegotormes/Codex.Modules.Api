using Eternet.Accounting.Api.Features.JournalEntries.Shared;
using Eternet.Accounting.Api.Model;
using Eternet.Accounting.Api.Tests.Common;
using Eternet.Accounting.Contracts.JournalEntries.Responses;

namespace Eternet.Accounting.Api.Tests.Features.Steps;

public class CheckActiveAccountingPeriodAsyncTests()
{
    private readonly AccountingContextFixture _fixture = new();

    [Fact]
    public async Task Handle_WithValidPeriod_ReturnsPeriod()
    {
        await using var transactional = await _fixture.CreateContextAsync();
        var context = transactional.Context;
        var period = new AccountingPeriod
        {
            Id = 1,
            Description = "2024",
            StartDate = new DateOnly(2024, 1, 1),
            EndDate = new DateOnly(2024, 12, 31)
        };
        context.AccountingPeriods.Add(period);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var step = new CheckActiveAccountingPeriodAsync(context);
        var result = await step.Handle(new DateOnly(2024, 6, 1), JournalEntryPeriodStatus.Current, CancellationToken.None);

        result.IsResult.Should().BeTrue();
        result.TryGetResult(out var value).Should().BeTrue();
        value.Should().Be(period);
    }

    [Fact]
    public async Task Handle_WithInvalidStatus_ReturnsError()
    {
        await using var transactional = await _fixture.CreateContextAsync();
        var context = transactional.Context;
        var step = new CheckActiveAccountingPeriodAsync(context);

        var result = await step.Handle(new DateOnly(2024, 1, 1), (JournalEntryPeriodStatus)999, CancellationToken.None);

        result.IsInvalidStateError.Should().BeTrue();
    }
}

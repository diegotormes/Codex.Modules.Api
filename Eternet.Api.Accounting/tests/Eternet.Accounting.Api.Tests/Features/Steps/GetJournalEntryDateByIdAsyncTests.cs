using Eternet.Accounting.Api.Features.JournalEntries.Shared;
using Eternet.Accounting.Api.Model;
using Eternet.Accounting.Api.Tests.Common;

namespace Eternet.Accounting.Api.Tests.Features.Steps;

public class GetJournalEntryDateByIdAsyncTests()
{
    private readonly AccountingContextFixture _fixture = new();

    [Fact]
    public async Task Handle_WhenEntryExists_ReturnsDate()
    {
        await using var transactional = await _fixture.CreateContextAsync();
        var context = transactional.Context;
        var entry = new JournalEntry { Id = 1, Guid = Guid.NewGuid(), Date = new DateOnly(2024, 1, 2), Description = "d" };
        context.JournalEntries.Add(entry);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var step = new GetJournalEntryDateByIdAsync(context);
        var result = await step.Handle(entry.Guid, CancellationToken.None);

        result.IsResult.Should().BeTrue();
        result.TryGetResult(out var value).Should().BeTrue();
        value.Should().Be(entry.Date);
    }

    [Fact]
    public async Task Handle_WhenEntryMissing_ReturnsNotFound()
    {
        await using var transactional = await _fixture.CreateContextAsync();
        var context = transactional.Context;
        var step = new GetJournalEntryDateByIdAsync(context);
        var result = await step.Handle(Guid.NewGuid(), CancellationToken.None);
        result.IsNotFoundError.Should().BeTrue();
    }
}

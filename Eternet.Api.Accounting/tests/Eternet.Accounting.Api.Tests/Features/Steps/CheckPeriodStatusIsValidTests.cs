using Eternet.Accounting.Api.Features.JournalEntries.CreateEntry.Steps;
using Eternet.Accounting.Api.Model;

namespace Eternet.Accounting.Api.Tests.Features.Steps;

public class CheckPeriodStatusIsValidTests
{
    [Fact]
    public void Handle_WhenDateAfterPartialClose_ReturnsInvalidState()
    {
        var step = new CheckPeriodStatusIsValid();
        var period = new AccountingPeriod
        {
            Description = "2024",
            StartDate = new DateOnly(2024,1,1),
            EndDate = new DateOnly(2024,12,31),
            PartialCloseDate = new DateOnly(2024,3,31)
        };

        var result = step.Handle(period, new DateOnly(2024,4,1));

        result.IsInvalidStateError.Should().BeTrue();
        result.IsNext.Should().BeFalse();
    }

    [Fact]
    public void Handle_WhenDateValid_ReturnsNext()
    {
        var step = new CheckPeriodStatusIsValid();
        var period = new AccountingPeriod
        {
            Description = "2024",
            StartDate = new DateOnly(2024,1,1),
            EndDate = new DateOnly(2024,12,31)
        };

        var result = step.Handle(period, new DateOnly(2024,2,1));

        result.IsNext.Should().BeTrue();
        result.IsInvalidStateError.Should().BeFalse();
    }
}

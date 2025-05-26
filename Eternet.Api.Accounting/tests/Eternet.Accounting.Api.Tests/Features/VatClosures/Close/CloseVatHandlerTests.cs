using Eternet.Accounting.Api.Features.VatClosures.Close;
using Eternet.Accounting.Contracts.VatClosures;

namespace Eternet.Accounting.Api.Tests.Features.VatClosures.Close;

public class CloseVatHandlerTests
{
    [Fact]
    public void Handle_ReturnsCloseDate()
    {
        var handler = new CloseVatHandler();
        var request = new CloseVat.Request
        {
            Month = 5,
            Year = 2024,
            CloseDate = new DateOnly(2024,5,31),
            DueDate = new DateOnly(2024,6,15)
        };

        var response = handler.Handle(request);

        response.CloseDate.Should().Be(request.CloseDate);
    }
}

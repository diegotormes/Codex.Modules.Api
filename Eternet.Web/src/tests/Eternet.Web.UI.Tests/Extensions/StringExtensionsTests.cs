using Eternet.Web.UI.Extensions;
using FluentAssertions;

namespace Eternet.Web.UI.Tests.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("abc123", "123")]
    [InlineData("abc", null)]
    [InlineData(null, null)]
    public void GetNumericValue_ReturnsDigits(string? input, string? expected)
    {
        var result = input.GetNumericValue();

        result.Should().Be(expected);
    }
}

using AwesomeAssertions;

namespace Eternet.Accounting.Api.Tests.Assertions;

public static class DomainResultAssertions
{
    public static void ShouldBeSuccess(this object result)
    {
        var type = result.GetType();
        var isSuccess = (bool?)type.GetProperty("IsDomainResult")?.GetValue(result) ?? false;
        var message = type.GetMethod("GetErrorMessage")?.Invoke(result, null) as string;
        isSuccess.Should().BeTrue(message);
    }
}

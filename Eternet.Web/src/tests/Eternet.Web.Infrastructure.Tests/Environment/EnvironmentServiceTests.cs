using Microsoft.AspNetCore.Http;
using Eternet.Web.Infrastructure.Environment;
using FluentAssertions;

namespace Eternet.Web.Infrastructure.Tests.Environment;

public class EnvironmentServiceTests
{
    [Fact]
    public void ReturnsProductionWhenHttpContextIsNull()
    {
        var accessor = new HttpContextAccessor { HttpContext = null };
        var service = new EnvironmentService(accessor);

        var result = service.GetEnvironment();

        result.Should().Be(ApiEnvironment.Production);
    }

    [Theory]
    [InlineData("Env", "Prod", ApiEnvironment.Production)]
    [InlineData("Env", "Production", ApiEnvironment.Production)]
    [InlineData("Env", "Test", ApiEnvironment.Testing)]
    [InlineData("Env", "Testing", ApiEnvironment.Testing)]
    [InlineData("X-Environment", "Prod", ApiEnvironment.Production)]
    [InlineData("X-Environment", "Production", ApiEnvironment.Production)]
    [InlineData("X-Environment", "Test", ApiEnvironment.Testing)]
    [InlineData("X-Environment", "Testing", ApiEnvironment.Testing)]
    public void DetectsEnvironmentFromHeaders(string header, string value, ApiEnvironment expected)
    {
        var context = new DefaultHttpContext();
        context.Request.Headers[header] = value;
        var accessor = new HttpContextAccessor { HttpContext = context };
        var service = new EnvironmentService(accessor);

        var result = service.GetEnvironment();

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("Prod", ApiEnvironment.Production)]
    [InlineData("Production", ApiEnvironment.Production)]
    [InlineData("Test", ApiEnvironment.Testing)]
    [InlineData("Testing", ApiEnvironment.Testing)]
    public void DetectsEnvironmentFromQuery(string value, ApiEnvironment expected)
    {
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString($"?Env={value}");
        var accessor = new HttpContextAccessor { HttpContext = context };
        var service = new EnvironmentService(accessor);

        var result = service.GetEnvironment();

        result.Should().Be(expected);
    }

    [Fact]
    public void Header_overrides_query_value()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers["Env"] = "Prod";
        context.Request.QueryString = new QueryString("?Env=Test");
        var accessor = new HttpContextAccessor { HttpContext = context };
        var service = new EnvironmentService(accessor);

        var result = service.GetEnvironment();

        result.Should().Be(ApiEnvironment.Production);
    }

    [Fact]
    public void ThrowsExceptionOnInvalidEnvironmentValue()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers["Env"] = "Unknown";
        var accessor = new HttpContextAccessor { HttpContext = context };
        var service = new EnvironmentService(accessor);

        FluentActions.Invoking(() => service.GetEnvironment()).Should().Throw<Exception>();
    }
}

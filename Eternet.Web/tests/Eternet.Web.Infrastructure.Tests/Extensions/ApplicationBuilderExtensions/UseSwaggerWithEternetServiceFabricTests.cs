using System.Text.Json;
using Eternet.Web.Infrastructure.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Eternet.Web.Infrastructure.Tests.Extensions.ApplicationBuilderExtensions;

static class SwaggerTestHelper
{
    public static async Task<(List<string?> Servers, string Scheme)> GetServersAsync<TProgram>()
        where TProgram : class
    {
        await using var factory = new WebApplicationFactory<TProgram>();
        using var client = factory.CreateClient();

        var json = await client.GetStringAsync("/swagger/v1/swagger.json");
        using var doc = JsonDocument.Parse(json);

        var servers = doc.RootElement.GetProperty("servers")
            .EnumerateArray()
            .Select(e => e.GetProperty("url").GetString())
            .ToList();

        return (servers, client.BaseAddress.Scheme);
    }
}

public class UseSwaggerWithEternetServiceFabricTests
{
    [Fact]
    public async Task UseSwaggerWithEternetServiceFabric_ServiceFabricEnabled_AddsServiceFabricAndCustomServers()
    {
        var (servers, scheme) = await SwaggerTestHelper.GetServersAsync<Program>();

        servers.Should().Contain($"{scheme}://servicefabric.eternet.cc:19081/TestApp");
        servers.Should().Contain($"{scheme}://localhost:19081/TestApp");
        servers.Should().Contain("https://api.test.com");
    }

    [Fact]
    public async Task UseSwaggerWithEternetServiceFabric_ServiceFabricDisabled_DoesNotAddServiceFabricServers()
    {
        var (servers, _) = await SwaggerTestHelper.GetServersAsync<ProgramWithoutServiceFabric>();

        servers.Should().NotContain(s => s!.Contains("servicefabric", StringComparison.OrdinalIgnoreCase));
        servers.Should().NotContain("https://api.test.com");
    }

    [Fact]
    public async Task UseSwaggerWithEternetServiceFabric_NoCustomServers_AddsOnlyServiceFabricServers()
    {
        var (servers, scheme) = await SwaggerTestHelper.GetServersAsync<ProgramServiceFabricOnly>();

        servers.Should().Equal(new[]
        {
            $"{scheme}://servicefabric.eternet.cc:19081/TestApp",
            $"{scheme}://localhost:19081/TestApp"
        });
    }
}

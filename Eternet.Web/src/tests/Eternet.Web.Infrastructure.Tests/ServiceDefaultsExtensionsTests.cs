using Eternet.Web.Infrastructure.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Eternet.Web.Infrastructure.Tests;

public class ServiceDefaultsExtensionsTests
{
    [Fact]
    public void MapDefaultEndpoints_in_development_maps_health_routes()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = Environments.Development;
        builder.Services.AddHealthChecks();
        var app = builder.Build();

        app.MapDefaultEndpoints();
        app.UseRouting();
        app.UseEndpoints(_ => { });

        var endpoints = app.Services.GetRequiredService<EndpointDataSource>().Endpoints;
        endpoints.Should().Contain(e => CreateRouteEndpointPredicate("/health")(e));
        endpoints.Should().Contain(e => CreateRouteEndpointPredicate("/alive")(e));
    }

    [Fact]
    public void MapDefaultEndpoints_in_production_does_not_map_health_routes()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = Environments.Production;
        builder.Services.AddHealthChecks();
        var app = builder.Build();

        app.MapDefaultEndpoints();
        app.UseRouting();
        app.UseEndpoints(_ => { });

        var endpoints = app.Services.GetRequiredService<EndpointDataSource>().Endpoints;
        endpoints.Should().NotContain(e => CreateRouteEndpointPredicate("/health")(e));
        endpoints.Should().NotContain(e => CreateRouteEndpointPredicate("/alive")(e));
    }

    private static Predicate<Endpoint> CreateRouteEndpointPredicate(string routePattern)
    {
        return e => e is RouteEndpoint re && re.RoutePattern.RawText == routePattern;
    }
}

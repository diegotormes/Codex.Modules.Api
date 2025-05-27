using System.Text.Json.Serialization;
using Eternet.Api.Common.Extensions;
using Eternet.AspNetCore.ServiceFabric;
using Eternet.Web.Infrastructure.Extensions;

namespace TemplateService.Api;

public static class ProgramExtensions
{
    public static void AddAppServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.SetupMvcFormatters();
        services.AddEndpointsApiExplorer();
        services.AddCustomSwagger();
        services.AddHealthChecks();
    }

    public static void UseAppMiddlewares(this WebApplication app)
    {
        app.UseSharedSwaggerUI(
            serviceFabricPath: "Eternet.Api.Modules/TemplateService.Api",
            useServiceFabric: ServiceFabricUtils.IsHosted);
        app.MapControllers();
        app.MapGet("/hello-world", () => Results.Ok("Hello, world!"));
    }
}

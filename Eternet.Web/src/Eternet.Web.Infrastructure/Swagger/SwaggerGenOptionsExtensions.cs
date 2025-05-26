using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Eternet.Web.Infrastructure.Swagger;

public static class SwaggerGenOptionsExtensions
{
    public static void MapTimeSpan(this SwaggerGenOptions c)
    {
        c.MapType<TimeSpan>(() => new() { Type = "string", Example = new OpenApiString("00:00:00"), Format = "time-span" });
    }

    public static void MapTimeOnly(this SwaggerGenOptions c)
    {
        c.MapType<TimeOnly>(() => new() { Type = "string", Example = new OpenApiString("00:00:00"), Format = "time-only" });
    }

}
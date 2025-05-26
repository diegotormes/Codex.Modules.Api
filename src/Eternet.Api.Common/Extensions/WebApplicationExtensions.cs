using Eternet.AspNetCore.ServiceFabric;
using Microsoft.AspNetCore.Builder;

namespace Eternet.Api.Common.Extensions;

public static class WebApplicationExtensions
{
    public static void UseAppMiddlewares(this WebApplication app, string serviceFabricPath, bool mapDefaultEndpoints = false)
    {
        app.UseSharedSwaggerUI(serviceFabricPath: serviceFabricPath, useServiceFabric: ServiceFabricUtils.IsHosted);
        app.MapControllers();
        if (mapDefaultEndpoints)
        {
            app.MapDefaultEndpoints();
        }
    }
}

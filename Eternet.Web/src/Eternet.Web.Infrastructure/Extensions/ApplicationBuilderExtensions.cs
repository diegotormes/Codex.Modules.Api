using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

namespace Eternet.Web.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerWithEternetServiceFabric(
        this IApplicationBuilder app,
        string serviceFabricPath,
        bool useServiceFabric = false,
        params string[] additionalServers)
    {
        app.UseSwagger(swaggerOptions =>
        {
            if (useServiceFabric is false)
            {
                return;
            }
            swaggerOptions.RouteTemplate = "swagger/{documentName}/swagger.json";

            swaggerOptions.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
            {
                swaggerDoc.Servers = [];

                foreach (var additionalServer in additionalServers)
                {
                    swaggerDoc.Servers.Add(new OpenApiServer
                    {
                        Url = additionalServer
                    });
                }

                swaggerDoc.Servers.Add(new OpenApiServer
                {
                    Url = $"{httpReq.Scheme}://servicefabric.eternet.cc:19081/{serviceFabricPath}"
                });

                swaggerDoc.Servers.Add(new OpenApiServer
                {
                    Url = $"{httpReq.Scheme}://localhost:19081/{serviceFabricPath}"
                });
            });
        });

        return app;
    }
}

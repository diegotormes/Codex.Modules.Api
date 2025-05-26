using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Eternet.Web.Infrastructure.Swagger;

public class RemoveODataMediaTypesFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        bool isOData = typeof(ODataController).IsAssignableFrom(context.MethodInfo.DeclaringType);

        if (isOData)
        {
            return;
        }

        Strip(operation.RequestBody?.Content);
        foreach (var resp in operation.Responses.Values)
        {
            Strip(resp.Content);
        }
    }

    private static void Strip(IDictionary<string, OpenApiMediaType>? content)
    {
        if (content is null)
        {
            return;
        }

        var toRemove = content.Keys.Where(k => k.Contains("odata", StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var k in toRemove)
        {
            content.Remove(k);
        }
    }
}
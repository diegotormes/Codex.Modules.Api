using Eternet.Web.Infrastructure.Environment;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Eternet.Web.Infrastructure.Swagger;

public class EnvHeaderDocumentFilter(EnvType defaultEnv) : IDocumentFilter
{
    public void Apply(OpenApiDocument doc, DocumentFilterContext _)
    {
        foreach (var op in doc.Paths.Values.SelectMany(p => p.Operations.Values))
        {
            if (op.Parameters.Any(p => p.Name == HttpConstants.Env &&
                                       p.In == ParameterLocation.Header))
            {
                continue;
            }

            op.Parameters.Add(new OpenApiParameter
            {
                Name = HttpConstants.Env,
                In = ParameterLocation.Header,
                Description = "Environment",
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Enum = [.. Enum.GetNames<EnvType>().Select(n => new OpenApiString(n))],
                    Default = new OpenApiString(defaultEnv.ToString())
                }
            });
        }
    }
}


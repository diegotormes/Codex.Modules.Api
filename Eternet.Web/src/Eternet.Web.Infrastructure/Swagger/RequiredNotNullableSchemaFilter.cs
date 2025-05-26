using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Eternet.Web.Infrastructure.Swagger;

public class RequiredNotNullableSchemaFilter : ISchemaFilter
{
    private static Type _attributeType { get; } = typeof(System.Runtime.CompilerServices.RequiredMemberAttribute);

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null)
        {
            return;
        }
        foreach (var property in schema.Properties)
        {
            var field = context.Type
                .GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(x =>
                    string.Equals(x.Name, property.Key, StringComparison.InvariantCultureIgnoreCase));

            if (field is not null && Attribute.IsDefined(field, _attributeType))
            {
                property.Value.Nullable = false;
                continue;
            }
        }
    }
}

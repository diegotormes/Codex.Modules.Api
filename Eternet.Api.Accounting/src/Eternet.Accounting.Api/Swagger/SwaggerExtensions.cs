using Eternet.Web.Infrastructure.Swagger;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Eternet.Accounting.Api.Swagger;

public static class SwaggerExtensions
{
    public static void AddCustomSwagger_ToRemove(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SupportNonNullableReferenceTypes();
            c.UseAllOfToExtendReferenceSchemas();
            c.SchemaFilter<RequiredNotNullableSchemaFilter>();
            c.DocumentFilter<EnvHeaderDocumentFilter>(EnvType.Test);
            c.MapTimeSpan();
            c.MapTimeOnly();
            c.OperationFilter<ReadGeneratedCodeAttributesOperationFilter>();
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            c.OperationFilter<RemoveODataMediaTypesFilter>();
            //var xml = Path.Combine(AppContext.BaseDirectory,
            //               "Eternet.Accounting.Contracts.xml");
            //c.IncludeXmlComments(xml, includeControllerXmlComments: false);
        });
    }
}

public class ReadGeneratedCodeAttributesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var meta = context.ApiDescription.ActionDescriptor.EndpointMetadata
                              .OfType<IEndpointSummaryMetadata>()
                              .FirstOrDefault();

        if (meta is not null && string.IsNullOrEmpty(operation.Summary))
        {
            operation.Summary = meta.Summary;
        }
    }
}

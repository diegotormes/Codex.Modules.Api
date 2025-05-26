using Eternet.Web.Infrastructure.Swagger;
using FluentAssertions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Eternet.Web.Infrastructure.Tests.Swagger;

public class RequiredNotNullableSchemaFilterTests
{
    private class Sample
    {
        public required string? RequiredProp { get; set; }
        public string? OptionalProp { get; set; }
    }

    [Fact]
    public void Required_properties_are_marked_non_nullable()
    {
        var schema = new OpenApiSchema
        {
            Properties = new Dictionary<string, OpenApiSchema>
            {
                ["RequiredProp"] = new OpenApiSchema { Nullable = true },
                ["OptionalProp"] = new OpenApiSchema { Nullable = true }
            }
        };

        var context = new SchemaFilterContext(typeof(Sample), null!, null!, null!);
        var filter = new RequiredNotNullableSchemaFilter();

        filter.Apply(schema, context);

        schema.Properties["RequiredProp"].Nullable.Should().BeFalse();
        schema.Properties["OptionalProp"].Nullable.Should().BeTrue();
    }
}

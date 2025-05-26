using Eternet.Web.Infrastructure.Environment;
using Eternet.Web.Infrastructure.Swagger;
using FluentAssertions;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Eternet.Web.Infrastructure.Tests.Swagger;

public class EnvHeaderDocumentFilterTests
{
    [Fact]
    public void Adds_env_header_parameter_when_missing()
    {
        var doc = new OpenApiDocument
        {
            Paths = new OpenApiPaths
            {
                ["/test"] = new OpenApiPathItem
                {
                    Operations =
                    {
                        [OperationType.Get] = new OpenApiOperation()
                    }
                }
            }
        };

        var filter = new EnvHeaderDocumentFilter(EnvType.Prod);

        filter.Apply(doc, null!);

        var operation = doc.Paths["/test"].Operations[OperationType.Get];
        var parameter = operation.Parameters.Single(p => p.Name == HttpConstants.Env);
        parameter.In.Should().Be(ParameterLocation.Header);
        parameter.Schema.Enum.Select(v => ((OpenApiString)v).Value)
                .Should().BeEquivalentTo(["Test", "Prod"]);
        ((OpenApiString)parameter.Schema.Default).Value.Should().Be("Prod");
    }
}

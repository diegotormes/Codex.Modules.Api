using Eternet.Web.Infrastructure.Swagger;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Eternet.Web.Infrastructure.Tests.Swagger;

public class RemoveODataMediaTypesFilterTests
{
    private static OperationFilterContext CreateContext<T>() where T : ControllerBase
    {
        var method = typeof(T).GetMethod(nameof(TestController.Get))!;
        return new OperationFilterContext(new ApiDescription(), null!, new SchemaRepository(), method);
    }

    [Fact]
    public void Removes_odata_media_types_for_non_odata_controllers()
    {
        var operation = new OpenApiOperation
        {
            RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType(),
                    ["application/prs.odata"] = new OpenApiMediaType()
                }
            },
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType(),
                        ["application/prs.odata"] = new OpenApiMediaType()
                    }
                }
            }
        };

        var filter = new RemoveODataMediaTypesFilter();
        var context = CreateContext<TestController>();

        filter.Apply(operation, context);

        operation.RequestBody!.Content.Keys.Should().BeEquivalentTo("application/json");
        operation.Responses["200"].Content.Keys.Should().BeEquivalentTo("application/json");
    }

    [Fact]
    public void Keeps_odata_media_types_for_odata_controllers()
    {
        var operation = new OpenApiOperation
        {
            RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType(),
                    ["application/prs.odata"] = new OpenApiMediaType()
                }
            }
        };

        var filter = new RemoveODataMediaTypesFilter();
        var context = CreateContext<ODataTestController>();

        filter.Apply(operation, context);

        operation.RequestBody!.Content.Keys.Should().Contain(["application/json", "application/prs.odata"]);
    }

    private class TestController : ControllerBase
    {
        public static void Get() { }
    }

    private class ODataTestController : ODataController
    {
        public static void Get() { }
    }
}

using System.Text;
using Eternet.Web.Infrastructure.Extensions;
using FluentAssertions;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Eternet.Web.Infrastructure.Tests;

public class SwaggerUiFilterTests
{
    [Fact]
    public void AddPlugins_injects_search_script()
    {
        var opt = new SwaggerUIOptions
        {
            IndexStream = () => new MemoryStream(Encoding.UTF8.GetBytes("<html><body></body></html>"))
        };

        opt.AddPlugins();
        using var stream = opt.IndexStream();
        stream.Position = 0;
        var html = new StreamReader(stream).ReadToEnd();

        html.Should().Contain("_content/Eternet.Web.Infrastructure/swagger-ui/plugin-search/index.js");
        html.Should().Contain("cfg.filter");
    }
}

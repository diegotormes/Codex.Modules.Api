using Eternet.Web.Infrastructure.Extensions;
using Eternet.Web.Infrastructure.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using FluentAssertions;

namespace Eternet.Web.Infrastructure.Tests;

public class MvcFormatterSetupTests
{
    [Fact]
    public void SetupMvcFormatters_ConfiguresJsonOnlyFormatters()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddControllers();
        services.SetupMvcFormatters();

        // Act
        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<MvcOptions>>().Value;

        // Assert XmlOnlyMetadataOutputFormatter exists
        options.OutputFormatters.Should().ContainSingle(f => f is XmlOnlyMetadataOutputFormatter);

        // Assert output formatters media types
        foreach (var formatter in options.OutputFormatters.OfType<OutputFormatter>())
        {
            var mediaTypes = formatter.SupportedMediaTypes.Select(m => m.ToString()).ToArray();
            if (formatter is XmlOnlyMetadataOutputFormatter)
            {
                mediaTypes.Should().Equal(["application/xml"]);
            }
            else
            {
                mediaTypes.Should().Equal(["application/json"]);
            }
        }

        // Assert input formatters media types
        foreach (var formatter in options.InputFormatters.OfType<InputFormatter>())
        {
            var mediaTypes = formatter.SupportedMediaTypes.Select(m => m.ToString()).ToArray();
            mediaTypes.Should().Equal(["application/json"]);
        }
    }
}

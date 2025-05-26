using Eternet.Web.Infrastructure.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.Extensions.DependencyInjection;

namespace Eternet.Web.Infrastructure.Extensions;

public static class MvcExtensions
{
    public static void SetupMvcFormatters(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(mvc =>
        {
            foreach (var f in mvc.OutputFormatters)
            {
                if (f is ODataOutputFormatter oDataOutputFormatter)
                {
                    oDataOutputFormatter.SupportedMediaTypes.Clear();
                    oDataOutputFormatter.SupportedMediaTypes.Add("application/json;odata.metadata=minimal");
                    continue;
                }
                if(f is OutputFormatter outputFormatter)
                {
                    outputFormatter.SupportedMediaTypes.Clear();
                    outputFormatter.SupportedMediaTypes.Add("application/json");
                }
            }
            mvc.OutputFormatters.Add(new XmlOnlyMetadataOutputFormatter());
            foreach (var f in mvc.InputFormatters)
            {
                if (f is ODataInputFormatter oDataInputFormatter)
                {
                    oDataInputFormatter.SupportedMediaTypes.Clear();
                    oDataInputFormatter.SupportedMediaTypes.Add("application/json");
                    continue;
                }
                if (f is InputFormatter inputFormatter)
                {
                    inputFormatter.SupportedMediaTypes.Clear();
                    inputFormatter.SupportedMediaTypes.Add("application/json");
                }
            }
        });       

    }
}
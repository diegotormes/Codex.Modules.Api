using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.Net.Http.Headers;
using Microsoft.OData;
using Microsoft.OData.Edm;
using System.Text;

namespace Eternet.Web.Infrastructure.Formatters;

public sealed class XmlOnlyMetadataOutputFormatter : ODataOutputFormatter
{
    private static readonly MediaTypeHeaderValue Xml =
        MediaTypeHeaderValue.Parse("application/xml");

    public XmlOnlyMetadataOutputFormatter() : base([ODataPayloadKind.MetadataDocument])
    {
        SupportedMediaTypes.Clear();
        SupportedMediaTypes.Add(Xml);

        SupportedEncodings.Clear();
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type) 
        => typeof(IEdmModel).IsAssignableFrom(type);

    public override bool CanWriteResult(OutputFormatterCanWriteContext ctx)
        => base.CanWriteResult(ctx) && CanWriteType(ctx.ObjectType);
}

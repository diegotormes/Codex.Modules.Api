using System.Text;
using Eternet.Web.Infrastructure.Formatters;
using FluentAssertions;

namespace Eternet.Web.Infrastructure.Tests;

public sealed class XmlOnlyMetadataOutputFormatterTests
{
    [Fact]
    public void SupportedMediaTypes_ContainsOnlyApplicationXml()
    {
        var formatter = new XmlOnlyMetadataOutputFormatter();

        formatter.SupportedMediaTypes.Should().ContainSingle()
            .Which.ToString().Should().Be("application/xml");
    }

    [Fact]
    public void SupportedEncodings_ContainsUtf8AndUnicode()
    {
        var formatter = new XmlOnlyMetadataOutputFormatter();

        formatter.SupportedEncodings.Should().Contain([Encoding.UTF8, Encoding.Unicode]);
        formatter.SupportedEncodings.Should().HaveCount(2);
    }

    //private sealed class TestFormatter : XmlOnlyMetadataOutputFormatter
    //{
    //    public bool CallCanWriteType(Type type) => base.CanWriteType(type);
    //}

    //[Fact]
    //public void CanWriteType_ReturnsTrueOnlyForIEdmModelTypes()
    //{
    //    var formatter = new TestFormatter();

    //    Assert.True(formatter.CallCanWriteType(typeof(IEdmModel)));
    //    Assert.True(formatter.CallCanWriteType(typeof(EdmModel)));
    //    Assert.False(formatter.CallCanWriteType(typeof(object)));
    //}
}

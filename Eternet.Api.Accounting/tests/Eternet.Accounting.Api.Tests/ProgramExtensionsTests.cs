using Eternet.Accounting.Api.Configuration;
using Eternet.Accounting.Api.Extensions;

namespace Eternet.Accounting.Api.Tests;

public class ProgramExtensionsTests
{
    [Fact]
    public void CreateConnectionBuilder_CopiesProperties()
    {
        var builder = new LegacyConnectionStringBuilder
        {
            DataSource = "server",
            Database = "db",
            UserId = "user",
            Password = "pwd",
            Charset = "UTF8",
            Pooling = true,
            ConnectionLifeTime = 10,
            Port = 1234
        };

        var result = builder.CreateConnectionBuilder();

        result.DataSource.Should().Be(builder.DataSource);
        result.Database.Should().Be(builder.Database);
        result.UserID.Should().Be(builder.UserId);
        result.Password.Should().Be(builder.Password);
        result.Charset.Should().Be(builder.Charset);
        result.Pooling.Should().Be(builder.Pooling);
        result.ConnectionLifeTime.Should().Be(builder.ConnectionLifeTime);
        result.Port.Should().Be(builder.Port);
    }
}

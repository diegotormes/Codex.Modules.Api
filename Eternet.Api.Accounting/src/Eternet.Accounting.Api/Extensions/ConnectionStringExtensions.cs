using Eternet.Accounting.Api.Configuration;

namespace Eternet.Accounting.Api.Extensions;

public static class ConnectionStringExtensions
{
    public static FbConnectionStringBuilder CreateConnectionBuilder(this LegacyConnectionStringBuilder builder)
    {
        return new FbConnectionStringBuilder
        {
            DataSource = builder.DataSource,
            Database = builder.Database,
            UserID = builder.UserId,
            Password = builder.Password,
            Charset = builder.Charset,
            Pooling = builder.Pooling,
            ConnectionLifeTime = builder.ConnectionLifeTime,
            Port = builder.Port
        };
    }
}

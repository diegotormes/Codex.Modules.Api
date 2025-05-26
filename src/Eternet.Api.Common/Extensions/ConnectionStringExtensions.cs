using FirebirdSql.Data.FirebirdClient;

namespace Eternet.Api.Common.Extensions;

public static class ConnectionStringExtensions
{
    public static FbConnectionStringBuilder CreateConnectionBuilder(this ILegacyConnectionStringBuilder builder)
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

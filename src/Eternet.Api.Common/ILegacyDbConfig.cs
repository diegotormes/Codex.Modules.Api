namespace Eternet.Api.Common;

public interface ILegacyConnectionStringBuilder
{
    string DataSource { get; }
    string Database { get; }
    string UserId { get; }
    string Password { get; }
    string Charset { get; }
    bool Pooling { get; }
    int ConnectionLifeTime { get; }
    int Port { get; }
}

public interface ILegacyDbConfig
{
    bool UseProduction { get; set; }
    ILegacyConnectionStringBuilder Testing { get; set; }
    ILegacyConnectionStringBuilder Production { get; set; }
}

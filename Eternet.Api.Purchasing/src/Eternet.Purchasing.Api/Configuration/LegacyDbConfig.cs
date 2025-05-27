namespace Eternet.Purchasing.Api.Configuration;

public class LegacyDbConfig
{
    public bool UseProduction { get; set; } = false;
    public LegacyConnectionStringBuilder Testing { get; set; } = new();
    public LegacyConnectionStringBuilder Production { get; set; } = new();
}

public class LegacyConnectionStringBuilder
{
    public string DataSource { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Charset { get; set; } = string.Empty;
    public bool Pooling { get; set; }
    public bool LogEntityFramework { get; set; } = true;
    public int ConnectionLifeTime { get; set; }
    public int Port { get; set; }
}

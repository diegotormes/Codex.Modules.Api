using Eternet.Api.Common;

namespace Eternet.Purchasing.Api.Configuration;

public class LegacyDbConfig : ILegacyDbConfig
{
    public bool UseProduction { get; set; } = false;
    public ILegacyConnectionStringBuilder Testing { get; set; } = new LegacyConnectionStringBuilder();
    public ILegacyConnectionStringBuilder Production { get; set; } = new LegacyConnectionStringBuilder();
}

public class LegacyConnectionStringBuilder : ILegacyConnectionStringBuilder
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

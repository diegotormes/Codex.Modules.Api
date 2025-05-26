namespace Eternet.Purchasing.Api.Services;

public interface ILegacyContextFabric
{
    PurchasingContext NewAccountingContext(string connectionString, bool useProduction = false, bool logEntityFramework = true);
}

public class LegacyContextFabric(ILoggerFactory loggerFactory) : ILegacyContextFabric
{
    public PurchasingContext NewAccountingContext(string connectionString, bool useProduction = false, bool logEntityFramework = true)
    {
        var loggerFactoryLocal = logEntityFramework ? loggerFactory : null;
        return new PurchasingContext(GetOptions(connectionString), loggerFactory: loggerFactoryLocal);
    }

    private static DbContextOptions<PurchasingContext> GetOptions(string connectionString)
    {
        var options = new DbContextOptionsBuilder<PurchasingContext>();
        options.UseFirebird(connectionString);
        return options.Options;
    }
}
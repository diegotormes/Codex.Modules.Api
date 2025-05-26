namespace Eternet.Accounting.Api.Services;

public interface ILegacyContextFabric
{
    AccountingContext NewAccountingContext(string connectionString, bool useProduction = false, bool logEntityFramework = true);
}

public class LegacyContextFabric(ILoggerFactory loggerFactory) : ILegacyContextFabric
{
    public AccountingContext NewAccountingContext(string connectionString, bool useProduction = false, bool logEntityFramework = true)
    {
        var loggerFactoryLocal = logEntityFramework ? loggerFactory : null;
        return new AccountingContext(GetOptions(connectionString), loggerFactory: loggerFactoryLocal);
    }

    private static DbContextOptions<AccountingContext> GetOptions(string connectionString)
    {
        var options = new DbContextOptionsBuilder<AccountingContext>();
        options.UseFirebird(connectionString);
        return options.Options;
    }
}
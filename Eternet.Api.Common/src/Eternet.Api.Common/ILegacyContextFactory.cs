using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Eternet.Api.Common;

public interface ILegacyContextFactory<TContext> where TContext : DbContext
{
    TContext Create(string connectionString, bool logEntityFramework = true);
}

public class LegacyContextFactory<TContext>(ILoggerFactory loggerFactory) : ILegacyContextFactory<TContext>
    where TContext : DbContext
{
    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    public TContext Create(string connectionString, bool logEntityFramework = true)
    {
        var loggerFactoryLocal = logEntityFramework ? _loggerFactory : null;
        var options = new DbContextOptionsBuilder<TContext>()
            .UseFirebird(connectionString)
            .Options;
        return (TContext)Activator.CreateInstance(typeof(TContext), options, loggerFactoryLocal)!;
    }
}


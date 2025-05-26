using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Eternet.Api.Common;

namespace Eternet.Accounting.Api.Tests;

public class LegacyContextFactoryTests
{
    [Fact]
    public void Create_UsesProvidedConnectionString()
    {
        var factory = new LegacyContextFactory<AccountingContext>(new LoggerFactory());
        using var context = factory.Create("DataSource=server;Database=db", true);

        context.Database.GetConnectionString().Should().Contain("server");
    }

    [Fact]
    public void Create_WithoutLogging_UsesDifferentLoggerFactory()
    {
        var loggerFactory = new LoggerFactory();
        var factory = new LegacyContextFactory<AccountingContext>(loggerFactory);
        using var context = factory.Create("DataSource=server;Database=db", false);

        context.GetService<ILoggerFactory>().Should().NotBeSameAs(loggerFactory);
    }
}


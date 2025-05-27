using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Collections.Concurrent;
using Eternet.Crud.Relational.Behaviors;
using Eternet.Crud.Relational.Extensions;
using Eternet.Crud.Relational.Services;
using Eternet.Mediator.Services;
using Eternet.Mediator.Extensions;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Embedded;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Mediator;

namespace Eternet.Accounting.Api.Tests.Common;

public sealed class AccountingContextFixture : IAsyncLifetime
{
    private const int PoolSize = 3;
    private static readonly string s_poolRoot =
        Path.Combine(EternetFirebirdHelper.GetOutputDataPath(), "dbpool");

    private static readonly ConcurrentQueue<string> s_dbPool = new();
    private static readonly SemaphoreSlim s_semPool = new(PoolSize, PoolSize);
    private static readonly SemaphoreSlim s_semInit = new(1, 1);
    private static bool s_initialized;
    const bool ForceSqlite = false;
    private static readonly bool s_useSqlite =
        ForceSqlite ||
        OperatingSystem.IsLinux() ||
        !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ETERNET_TEST_USE_SQLITE"));

    public AccountingContextFixture()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        if (!s_useSqlite)
        {
            _ = EnsurePoolInitializedAsync();
        }
    }

    private static readonly SemaphoreSlim s_semTemplate = new(1, 1);
    private static readonly string s_templatePath =
        Path.Combine(EternetFirebirdHelper.GetOutputDataPath(),
        EternetFirebirdHelper.DbTestName);

    private static async Task EnsureTemplateAsync()
    {
        if (File.Exists(s_templatePath))
        {
            return;
        }
        await s_semTemplate.WaitAsync();
        try
        {
            if (!File.Exists(s_templatePath)) //Double check
            {
                var zip = Path.Combine(
                    EternetFirebirdHelper.GetOutputDataPath(),
                    EternetFirebirdHelper.DbZipFile);

                ZipFile.ExtractToDirectory(zip,
                    EternetFirebirdHelper.GetOutputDataPath(), overwriteFiles: true);
            }
        }
        finally
        {
            s_semTemplate.Release();
        }
    }
    private async Task EnsurePoolInitializedAsync()
    {
        await s_semInit.WaitAsync();
        try
        {
            if (s_initialized)
            {
                return;
            }

            await EnsureTemplateAsync();
            Directory.CreateDirectory(s_poolRoot);

            var tasks = Enumerable.Range(0, PoolSize)
                                  .Select(CreateOrReuseSlotAsync);
            var slotPaths = await Task.WhenAll(tasks);

            foreach (var path in slotPaths)
            {
                s_dbPool.Enqueue(path);
            }
            s_initialized = true;
        }
        finally
        {
            s_semInit.Release();
        }
    }

    private async Task<string> CreateOrReuseSlotAsync(int slot)
    {
        var slotPath = Path.Combine(s_poolRoot, $"slot_{slot}.fdb");

        if (File.Exists(slotPath))
        {
            return slotPath;
        }

        using var src = new FileStream(s_templatePath, FileMode.Open,
                                       FileAccess.Read, FileShare.Read);
        using var dst = new FileStream(slotPath, FileMode.CreateNew,
                                       FileAccess.Write, FileShare.None);
        await src.CopyToAsync(dst);

        return slotPath;
    }

    private static string GetConnectionString(string dbPath)
    {
        var native = FbNativeAssetManager.NativeAssetPath(FirebirdVersion.V3);
        var connectionBuilder = new FbConnectionStringBuilder
        {
            ClientLibrary = native,
            Database = dbPath,
            ServerType = FbServerType.Embedded,
            UserID = "SYSDBA"
        };
        return connectionBuilder.ToString();
    }

    public async Task<TransactionalTestContext> CreateContextAsync()
    {
        var services = CreateServices();
        if (s_useSqlite)
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();
            var dbOptions = new DbContextOptionsBuilder<AccountingContext>()
                .UseSqlite(connection)
                .Options;

            var ctx = new AccountingContext(dbOptions);
            await ctx.Database.EnsureCreatedAsync();
            var tx = await ctx.Database.BeginTransactionAsync();
            services.AddSingleton(ctx);
            services.AddSingleton<DbContext>(ctx);
            var sp = services.BuildServiceProvider();
            var mediator = sp.GetRequiredService<IMediator>();

            return new TransactionalTestContext(ctx, mediator, tx, string.Empty, _ => connection.Dispose());
        }

        await EnsurePoolInitializedAsync();
        await s_semPool.WaitAsync();
        if (!s_dbPool.TryDequeue(out var dbPath))
        {
            s_semPool.Release();
            throw new InvalidOperationException("Bad pool");
        }

        var optionsBuilder = new DbContextOptionsBuilder<AccountingContext>()
            .UseFirebird(GetConnectionString(dbPath))
            .Options;

        var ctxFb = new AccountingContext(optionsBuilder);
        var txFb = await ctxFb.Database.BeginTransactionAsync();
        var publishEventDDL = @"
            CREATE OR ALTER PROCEDURE PUBLISH_EVENT (
                NAMESPACE varchar(100) character set UTF8,
                EVENT_NAME varchar(100) character set UTF8,
                JSON varchar(3000),
                PUBLISH_ON_BUS smallint,
                ORIGIN varchar(32) character set UTF8,
                JSON_HEADER varchar(1000) = '')
            RETURNS (
                TRANSACTION_ID bigint)
            AS
            DECLARE VARIABLE RANDOM_ID bigint;
            BEGIN
                RANDOM_ID = CAST(1 + RAND() * 8999999 AS bigint);
                TRANSACTION_ID = RANDOM_ID;
                SUSPEND;
            END";
        await ctxFb.Database.ExecuteSqlRawAsync(publishEventDDL);
        services.AddSingleton(ctxFb);
        services.AddSingleton<DbContext>(ctxFb);
        var spFb = services.BuildServiceProvider();
        var mediatorFb = spFb.GetRequiredService<IMediator>();

        return new TransactionalTestContext(ctxFb, mediatorFb, txFb, dbPath, Release);
    }

    private static ServiceCollection CreateServices()
    {
        var servicesFb = new ServiceCollection();
        servicesFb.AddLogging();
        servicesFb.AddMediator(o => o.ServiceLifetime = ServiceLifetime.Scoped);
        servicesFb.RegisterMediatorServicesAndBehaviors();
        servicesFb.AddEternetMediatorStepsServices();
        servicesFb.AddEternetMediatorHandlersServices();
        var hybridCacheFb = NSubstitute.Substitute.For<Mediator.Caching.IHybridCache>();
        servicesFb.AddSingleton(hybridCacheFb);
        var optionsFb = new UnitOfWorkOptions { AutoCommit = false };
        servicesFb.AddSingleton<IHandlerEntityTypeResolver, GeneratedHandlerEntityTypeResolver>();
        servicesFb.AddEternetEntityFrameworkCoreCrud(optionsFb);
        servicesFb.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return servicesFb;
    }

    private static void Release(string dbPath)
    {
        s_dbPool.Enqueue(dbPath);
        s_semPool.Release();
    }

    private static void VacuumOldCopies(int days = 15)
    {
        foreach (var f in Directory.EnumerateFiles(s_poolRoot, "slot_*.fdb"))
        {
            var age = DateTime.UtcNow - File.GetLastWriteTimeUtc(f);
            if (age.TotalDays > days)
            {
                File.Delete(f);
            }
        }
    }

    public async ValueTask InitializeAsync()
    {
        if (!s_useSqlite)
        {
            await EnsurePoolInitializedAsync().ConfigureAwait(false);
        }
    }

    public ValueTask DisposeAsync()
    {
        VacuumOldCopies();
        return ValueTask.CompletedTask;
    }
}

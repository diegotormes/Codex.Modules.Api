using Eternet.Accounting.Api.Configuration;
using Eternet.Accounting.Api.Extensions;

namespace Eternet.Accounting.Api;
public static class ProgramExtensions
{
    public static string ViewsDatabase { get; } = "temp/all_views.db";

    private static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        var journal = builder.EntitySet<JournalEntryResponse>(Config.JournalEntries).EntityType;
        journal.HasKey(e => e.Id);
        builder.ComplexType<JournalEntryDetailResponse>();
        var generalLedgerAccount = builder.EntitySet<GeneralLedgerAccountResponse>(Config.GeneralLedgerAccounts).EntityType;
        generalLedgerAccount.HasKey(e => e.Id);

        var debit = builder.EntitySet<VatDebitResponse>(Config.VatDebits).EntityType;
        debit.HasKey(e => e.Id);
        var credit = builder.EntitySet<VatCreditResponse>(Config.VatCredits).EntityType;
        credit.HasKey(e => e.Id);
        var retention = builder.EntitySet<VatRetentionResponse>(Config.VatRetentions).EntityType;
        retention.HasKey(e => e.Id);
        var salary = builder.EntitySet<VatSalaryResponse>(Config.VatSalaries).EntityType;
        salary.HasKey(e => e.Id);
        return builder.GetEdmModel();
    }

    public static void AddAppServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddControllers()
            .AddOData(o =>
            {
                var emdModel = GetEdmModel();
                o.RouteOptions.EnableKeyAsSegment = true; // ODataUrlKeyDelimiter.Slash
                o.Select().Filter().OrderBy().Expand().SetMaxTop(100).Count()
                .AddRouteComponents(
                    Config.ControllerName, 
                    emdModel, 
                    routeServices => routeServices.AddSingleton<ISearchBinder, AllStringsSearchBinder>());
            })
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.SetupMvcFormatters();

        services.AddMediator(o =>
        {
            o.ServiceLifetime = ServiceLifetime.Scoped;
        });
        services.AddSingleton<IHybridCache, MyHybridCache>();
        services.AddSingleton<IHandlerEntityTypeResolver, GeneratedHandlerEntityTypeResolver>();
        services.RegisterMediatorServicesAndBehaviors();
        services.AddEternetMediatorStepsServices();
        services.AddEternetMediatorHandlersServices();
        services.AddEternetMediatorRequestSenders();
        services.AddEternetEntityFrameworkCoreCrud();
        services.AddEndpointsApiExplorer();
        services.AddCustomSwagger();
        services.AddConfigurations(configuration);

        services.AddSingleton<ILegacyContextFabric, LegacyContextFabric>();
        services.AddHttpContextAccessor();
        services.AddScoped<IEnvironmentService, EnvironmentService>();
        services.AddConnectionStringBuilder<LegacyDbConfig>(ignoreUseProduction: true);
        services.AddDbContext<ViewsContext>(o => o.UseSqlite($"Data Source={ViewsDatabase}"));
        services.AddHostedService<ViewsContextInitializer>();
        services.AddScoped(
            s =>
            {
                var legacyDbConfig = s.GetRequiredService<IOptions<LegacyDbConfig>>();
                var envService = s.GetRequiredService<IEnvironmentService>();
                var env = envService.GetEnvironment();
                var useProd = env == ApiEnvironment.Production;
                var connectionStringBuilder = s.GetRequiredService<FbConnectionStringBuilder>();
                var ctxFabric = s.GetRequiredService<ILegacyContextFabric>();
                var ctx = ctxFabric
                    .NewAccountingContext(
                        connectionStringBuilder.ToString(),
                        useProd,
                        logEntityFramework: true);
                return ctx;
            });
        services.AddScoped<DbContext>(s => s.GetService<AccountingContext>()!);
        services.AddValidatorsFromAssembly(typeof(CreateEntryAccountingJournal.Validator).Assembly);
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services
            .AddHealthChecks()
            .AddDbContextCheck<AccountingContext>(name: "firebird-db", tags: ["db", "firebird"]);
    }

    public static void UseAppMiddlewares(this WebApplication app)
    {
        app.UseSharedSwaggerUI(serviceFabricPath: "Eternet.Api.Modules/Eternet.Accounting.Api", useServiceFabric: ServiceFabricUtils.IsHosted);
        app.MapControllers();
        app.MapDefaultEndpoints();
    }

    private static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LegacyDbConfig>(configuration.GetSection(nameof(LegacyDbConfig)));
    }

}

using System.Text.Json.Serialization;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Options;
using Eternet.Mediator.Caching;
using Eternet.Mediator.Extensions;
using Eternet.Web.Infrastructure.Extensions;
using Eternet.Purchasing.Api.Configuration;
using Eternet.Purchasing.Api.Services;
using Eternet.Purchasing.Api.Extensions;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.Edm;
using Eternet.Purchasing.Contracts.Invoices;
using Eternet.Crud.Relational.Extensions;
using Eternet.Crud.Relational.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Eternet.Web.Infrastructure.Environment;
using Eternet.AspNetCore.ServiceFabric;

namespace Eternet.Purchasing.Api;
public class MyHybridCache : IHybridCache
{
    public string GetKey(string prefix, object[] parameters, string suffix = "")
    {
        return "";
    }

    public ValueTask<T> GetOrCreateAsync<TState, T>(string key, TState state, Func<TState, CancellationToken, ValueTask<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(default(T)!);
    }

    public ValueTask<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(default(T)!);
    }
}

public static class ProgramExtensions
{
    private static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        var invoiceEntity = builder.EntitySet<InvoiceResponse>(Config.Invoices).EntityType;
        invoiceEntity.HasKey(i => i.Id);
        builder.ComplexType<InvoiceDetailResponse>();
        builder.ComplexType<InvoiceRetentionResponse>();
        builder.ComplexType<InvoicePaymentMethodResponse>();
        builder.ComplexType<VendorCurrentAccountResponse>();
        return builder.GetEdmModel();
    }

    public static void AddAppServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddControllers().AddOData(o =>
        {
            var edmModel = GetEdmModel();
            o.Select().Filter().OrderBy().Expand().SetMaxTop(100).Count().AddRouteComponents(Config.ControllerName, edmModel);
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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
        services.AddConnectionStringBuilder<LegacyDbConfig>();
        services.AddScoped(s =>
        {
            var legacyDbConfig = s.GetRequiredService<IOptions<LegacyDbConfig>>();
            var envService = s.GetRequiredService<IEnvironmentService>();
            var env = envService.GetEnvironment();
            var useProd = env == ApiEnvironment.Production ? true : legacyDbConfig.Value.UseProduction;
            var connectionStringBuilder = s.GetRequiredService<FbConnectionStringBuilder>();
            var ctxFabric = s.GetRequiredService<ILegacyContextFabric>();
            var ctx = ctxFabric.NewAccountingContext(connectionStringBuilder.ToString(), useProd, logEntityFramework: true);
            return ctx;
        });
        services.AddScoped<DbContext>(s => s.GetService<PurchasingContext>()!);
        services.AddHealthChecks().AddDbContextCheck<PurchasingContext>(name: "firebird-db-purchasing", tags: ["db", "firebird"]);
    }

    public static void UseAppMiddlewares(this WebApplication app)
    {
        app.UseSharedSwaggerUI(serviceFabricPath: "Eternet.Api.Modules/Eternet.Purchasing.Api", useServiceFabric: ServiceFabricUtils.IsHosted);
        app.MapControllers();
    }

    private static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LegacyDbConfig>(configuration.GetSection(nameof(LegacyDbConfig)));
    }


}
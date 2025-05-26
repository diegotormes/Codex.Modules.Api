using System.Text;
using Eternet.AspNetCore.ServiceFabric.WebHost;
using Eternet.Web.Infrastructure.Extensions;
using OpenTelemetry.Trace;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services
    .AddOpenTelemetry()
    .WithTracing(t => t
        .AddSource("Eternet.Mediator")
        .AddEntityFrameworkCoreInstrumentation(opts =>
        {
            opts.SetDbStatementForText = true;
        }));

await builder.RunStatelessWebAsync(
    serviceNameType: "Eternet.Purchasing.ApiType",
    indexFormat: "eternet-purchasing-api",
    configureServices: b => b.Services.AddAppServices(b.Configuration, b.Environment),
    configureApp: app => app.UseAppMiddlewares()
);
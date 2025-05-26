using System.Text;
using Eternet.Accounting.Api;
using Eternet.AspNetCore.ServiceFabric.WebHost;
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
    serviceNameType: "Eternet.Accounting.ApiType",
    indexFormat: "eternet-accounting-api",
    configureServices: b => b.Services.AddAppServices(b.Configuration, b.Environment),
    configureApp: app => app.UseAppMiddlewares()
);

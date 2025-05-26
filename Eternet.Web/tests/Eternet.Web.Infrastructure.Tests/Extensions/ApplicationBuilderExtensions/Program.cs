using Eternet.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Eternet.Web.Infrastructure.Tests.Extensions.ApplicationBuilderExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwaggerWithEternetServiceFabric(
    serviceFabricPath: "TestApp",
    useServiceFabric: true,
    additionalServers: "https://api.test.com");

app.MapGet("/", () => "Hello");

app.Run();

public partial class Program { }

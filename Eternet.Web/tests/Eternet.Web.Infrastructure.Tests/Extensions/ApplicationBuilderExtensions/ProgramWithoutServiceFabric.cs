using Eternet.Web.Infrastructure.Extensions;

namespace Eternet.Web.Infrastructure.Tests.Extensions.ApplicationBuilderExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwaggerWithEternetServiceFabric("TestApp", useServiceFabric: false);

app.MapGet("/", () => "Hello");

app.Run();

public partial class ProgramWithoutServiceFabric { }

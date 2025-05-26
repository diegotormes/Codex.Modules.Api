using Eternet.Template.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppServices();

var app = builder.Build();
app.UseAppMiddlewares();

app.Run();

public partial class Program;

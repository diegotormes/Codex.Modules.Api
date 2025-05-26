namespace Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews;

public class ViewsContextInitializer(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var dbFilePath = Path.GetFullPath(ProgramExtensions.ViewsDatabase);
        var dbFolder = Path.GetDirectoryName(dbFilePath)!;
        if (!Directory.Exists(dbFolder))
        {
            Directory.CreateDirectory(dbFolder);
        }
        using var scope = serviceProvider.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<ViewsContext>();        
        await ctx.Database.EnsureCreatedAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

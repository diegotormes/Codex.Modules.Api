namespace Eternet.Template.Api;

public static class ProgramExtensions
{
    public static void AddAppServices(this IServiceCollection services)
    {
        ServiceCollectionExtensions.AddEternetApiCommon(services);
    }

    public static void UseAppMiddlewares(this WebApplication app)
    {
        ServiceCollectionExtensions.UseEternetApiCommon(app);
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace Eternet.Api.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEternetApiCommon(this IServiceCollection services)
    {
        services.AddControllers();
        return services;
    }

    public static void UseEternetApiCommon(this WebApplication app)
    {
        app.MapControllers();
    }
}

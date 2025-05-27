using Eternet.Web.Infrastructure.Environment;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Eternet.Api.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConnectionStringBuilder<TConfig>(this IServiceCollection services, bool ignoreUseProduction = false)
        where TConfig : class, ILegacyDbConfig
    {
        return services.AddTransient(provider =>
        {
            var options = provider.GetRequiredService<IOptions<TConfig>>();
            var envService = provider.GetRequiredService<IEnvironmentService>();
            var env = envService.GetEnvironment();
            var config = options.Value;

            if (env == ApiEnvironment.Testing)
            {
                return config.Testing.CreateConnectionBuilder();
            }

            if (!ignoreUseProduction && !config.UseProduction)
            {
                return config.Testing.CreateConnectionBuilder();
            }

            return config.Production.CreateConnectionBuilder();
        });
    }
}

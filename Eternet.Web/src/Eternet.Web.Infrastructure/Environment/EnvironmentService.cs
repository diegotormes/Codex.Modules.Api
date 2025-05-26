using Microsoft.AspNetCore.Http;

namespace Eternet.Web.Infrastructure.Environment;

public enum ApiEnvironment
{
    Testing,
    Production
}

public interface IEnvironmentService
{
    ApiEnvironment GetEnvironment();
}

public class EnvironmentService(IHttpContextAccessor httpContext)
    : IEnvironmentService
{
    public ApiEnvironment GetEnvironment()
    {
        if (httpContext.HttpContext is null)
        {
            return ApiEnvironment.Production;
        }

        var headers = httpContext.HttpContext.Request.Headers;
        var query = httpContext.HttpContext.Request.Query;

        if (headers.TryGetValue(HttpConstants.Env, out var env) ||
            headers.TryGetValue(HttpConstants.XEnvironment, out env) ||
            query.TryGetValue(HttpConstants.Env, out env))
        {
            return (string?)env switch
            {
                "Prod" => ApiEnvironment.Production,
                "Production" => ApiEnvironment.Production,
                "Test" => ApiEnvironment.Testing,
                "Testing" => ApiEnvironment.Testing,
                _ => throw new InvalidEnvironmentException($"The request contains an invalid Env variable in {nameof(EnvironmentService)}"),
            };
        }

        return ApiEnvironment.Production;
    }
}


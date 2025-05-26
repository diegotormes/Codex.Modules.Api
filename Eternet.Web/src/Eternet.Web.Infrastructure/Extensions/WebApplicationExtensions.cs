using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Eternet.Web.Infrastructure.Extensions;

public static class WebApplicationExtensions
{
    public static void UseSharedSwaggerUI(this WebApplication app,
        bool useSwaggerDarkMode = true,
        bool useUseStaticFiles = true,
        string? serviceFabricPath = null,
        string? additionalServers = null,
        string? azureAdClientId = null,
        bool useServiceFabric = false,
        Action<SwaggerUIOptions>? setupSwaggerUI = null)
    {
        if (useUseStaticFiles)
        {
            app.UseStaticFiles();
        }
        if (!string.IsNullOrEmpty(serviceFabricPath))
        {
            if (!string.IsNullOrEmpty(additionalServers))
            {
                app.UseSwaggerWithEternetServiceFabric(
                    serviceFabricPath: serviceFabricPath,
                    additionalServers: additionalServers,
                    useServiceFabric: useServiceFabric);
            }
            else
            {
                app.UseSwaggerWithEternetServiceFabric(serviceFabricPath: serviceFabricPath, useServiceFabric: useServiceFabric);
            }
        }
        app.UseSwaggerUI(c =>
        {
            c.AddPlugins();
            c.EnableFilter();
            if (useSwaggerDarkMode)
            {
                c.HeadContent += @"
<script>
(function () {
  var idx = location.pathname.indexOf('/swagger');
  var prefix = idx >= 0 ? location.pathname.substring(0, idx) : '';

  var dark = document.createElement('link');
  dark.rel = 'stylesheet';
  dark.href = prefix + '/_content/Eternet.Web.Infrastructure/swagger-ui/SwaggerDark.css';
  document.head.appendChild(dark);

  var compact = document.createElement('link');
  compact.rel = 'stylesheet';
  compact.href = prefix + '/_content/Eternet.Web.Infrastructure/swagger-ui/SwaggerCompact.css';
  document.head.appendChild(compact);

  ['plugin-search/index.js'].forEach(function (s) {
     var sc = document.createElement('script');
     sc.src = prefix + '/_content/Eternet.Web.Infrastructure/swagger-ui/' + s;
     document.head.appendChild(sc);
  });
})();
</script>

<script>
(function waitForBox () {
  const box = document.querySelector('.operation-filter-input');
  if (!box) { requestAnimationFrame(waitForBox); return; }
  box.placeholder = 'Filter…';
})();
</script>";

            }
            if (!string.IsNullOrEmpty(azureAdClientId))
            {
                c.OAuthClientId(azureAdClientId ?? "");
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            }
            setupSwaggerUI?.Invoke(c);
        });
    }
}

using System.Text;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Eternet.Web.Infrastructure.Extensions;

public static class SwaggerUIExtensions
{
    public static void AddPlugins(this SwaggerUIOptions opt)
    {
        var oldIndexStream = opt.IndexStream;

        opt.IndexStream = () =>
        {
            var stream = oldIndexStream();
            stream.Position = 0;
            var rdr = new StreamReader(stream);
            var html = rdr.ReadToEnd();

            const string marker = "</body>";
            const string injection = @"

<script src=""/_content/Eternet.Web.Infrastructure/swagger-ui/plugin-search/index.js""></script>

<script>
(function (w) {
  const Original = w.SwaggerUIBundle;

  function PatchedBundle(cfg = {}) {
    cfg.filter  = true;
    cfg.plugins = (cfg.plugins || [])
      .filter(p => p !== w.AdvancedFilterFullSearchPlugin)
      .concat(w.AdvancedFilterFullSearchPlugin);

    return Original(cfg);
  }

  Object.assign(PatchedBundle, Original);
  w.SwaggerUIBundle = PatchedBundle;
  console.log('[FullSearch] search box ready');
})(window);
</script>  
";

            html = html.Replace(marker, injection + marker);

            var newStream = new MemoryStream();
            var writer = new StreamWriter(newStream, Encoding.UTF8);
            foreach (var line in html.Split("\n"))
            {
                writer.Write($"{line}\n");
                writer.Flush();
            }

            newStream.Position = 0;

            return newStream;
        };

    }
}
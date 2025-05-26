//using System.Net;
//using System.Text.Json;
//using Microsoft.Extensions.Options;
//using Microsoft.ServiceFabric.Services.Client;
//using Yarp.ReverseProxy.Configuration;
//using Yarp.ReverseProxy.Forwarder;

//namespace Eternet.Web.Infrastructure.ServiceFabric;

//public interface IServiceFabricDiscovery
//{
//    Task<(IReadOnlyList<RouteConfig>, IReadOnlyList<ClusterConfig>)>
//        BuildAsync(CancellationToken ct);
//}

//public sealed class ServiceFabricDiscovery : IServiceFabricDiscovery, IDisposable
//{
//    private readonly ServicePartitionResolver _resolver = ServicePartitionResolver.GetDefault();
//    private readonly ReverseProxyOptions _opts;

//    public ServiceFabricDiscovery(IOptions<ReverseProxyOptions> opts) => _opts = opts.Value;

//    public async Task<(IReadOnlyList<RouteConfig>, IReadOnlyList<ClusterConfig>)>
//        BuildAsync(CancellationToken ct)
//    {
//        var routes = new List<RouteConfig>();
//        var clusters = new List<ClusterConfig>();

//        foreach (var svc in _opts.Services)
//        {
//            var dests = await ResolveDestinations(svc, ct);
//            if (dests.Count == 0)
//            {
//                continue;
//            }

//            clusters.Add(new ClusterConfig
//            {
//                ClusterId = svc.ClusterId,
//                Destinations = dests,
//                HttpRequest = svc.Http2
//                                ? new ForwarderRequestConfig
//                                {
//                                    Version = HttpVersion.Version20,
//                                    VersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
//                                }
//                                : null
//            });

//            routes.Add(new RouteConfig
//            {
//                RouteId = svc.RouteId,
//                ClusterId = svc.ClusterId,
//                Order = 0,
//                Match = new RouteMatch { Path = svc.Path, Methods = svc.Methods }
//            });
//        }
//        return (routes, clusters);
//    }

//    private async Task<Dictionary<string, DestinationConfig>> ResolveDestinations(
//            ServiceDescriptor svc, CancellationToken ct)
//    {
//        var map = new Dictionary<string, DestinationConfig>();
//        var part = await _resolver.ResolveAsync(
//                        new Uri(svc.ServiceUri), new ServicePartitionKey(), ct);

//        foreach (var ep in part.Endpoints.Where(e => e.Address.Contains(svc.EndpointName)))
//        {
//            try
//            {
//                using var doc = JsonDocument.Parse(ep.Address);
//                if (!doc.RootElement.TryGetProperty("Endpoints", out var eps) ||
//                    !eps.TryGetProperty(svc.EndpointName, out var value))
//                    continue;

//                var url = value.GetString()?.TrimEnd('/') + '/';
//                if (string.IsNullOrEmpty(url))
//                    continue;

//                map[$"{svc.ClusterId}-{Guid.NewGuid():N}".Substring(0, 8)] =
//                    new DestinationConfig { Address = url };
//            }
//            catch (JsonException) 
//            { 
//            }
//        }
//        return map;
//    }

//    public void Dispose() 
//    {  
//    }
//}

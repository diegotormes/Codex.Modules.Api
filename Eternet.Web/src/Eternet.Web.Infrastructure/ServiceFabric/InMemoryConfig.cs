//using System.Fabric;
//using Microsoft.Extensions.Primitives;
//using Yarp.ReverseProxy.Configuration;

//namespace Eternet.Web.Infrastructure.ServiceFabric;

//internal class InMemoryConfig : IProxyConfig
//{
//    public IReadOnlyList<RouteConfig> Routes { get; }
//    public IReadOnlyList<ClusterConfig> Clusters { get; }
//    public IChangeToken ChangeToken { get; } = new CancellationChangeToken(CancellationToken.None);

//    public InMemoryConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
//    {
//        Routes = routes;
//        Clusters = clusters;
//    }
//}

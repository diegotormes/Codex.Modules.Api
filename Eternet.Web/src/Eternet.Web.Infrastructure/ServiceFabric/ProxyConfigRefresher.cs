//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;

//namespace Eternet.Web.Infrastructure.ServiceFabric;

//public sealed class ProxyConfigRefresher : BackgroundService
//{
//    private readonly TimeSpan _period;
//    private readonly IServiceFabricDiscovery _discovery;
//    private readonly ServiceFabricProxyConfigProvider _provider;
//    private readonly ILogger<ProxyConfigRefresher> _log;

//    public ProxyConfigRefresher(IOptions<ReverseProxyOptions> opts,
//                                IServiceFabricDiscovery discovery,
//                                ServiceFabricProxyConfigProvider provider,
//                                ILogger<ProxyConfigRefresher> log)
//    {
//        _period = TimeSpan.FromSeconds(opts.Value.RefreshSeconds);
//        _discovery = discovery;
//        _provider = provider;
//        _log = log;
//    }

//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        while (!stoppingToken.IsCancellationRequested)
//        {
//            try
//            {
//                var (routes, clusters) = await _discovery.BuildAsync(stoppingToken);
//                _provider.Update(new InMemoryConfig(routes, clusters));
//            }
//            catch (Exception ex) 
//            { 
//                _log.LogWarning(ex, "Failed to refresh YARP proxy configuration");
//            }

//            await Task.Delay(_period, stoppingToken);
//        }
//    }
//}

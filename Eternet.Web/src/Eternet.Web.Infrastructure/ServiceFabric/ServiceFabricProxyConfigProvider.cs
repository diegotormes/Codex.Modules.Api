//using Microsoft.Extensions.Primitives;
//using Yarp.ReverseProxy.Configuration;

//namespace Eternet.Web.Infrastructure.ServiceFabric;

//public sealed class ServiceFabricProxyConfigProvider : IProxyConfigProvider, IDisposable
//{
//    private static readonly InMemoryConfig Empty = new([], []);

//    private readonly CancellationTokenSource _cts = new();
//    private volatile InMemoryConfig _current = Empty; 
//    private readonly ReloadToken _reload = new();

//    public IProxyConfig GetConfig() => _current;
//    public IChangeToken GetReloadToken() => _reload;

//    internal void Update(InMemoryConfig newConfig)
//    {
//        _current = newConfig;
//        _reload.OnReload();
//    }
//    public void Dispose() => _cts.Cancel();
//}

using Microsoft.Extensions.Primitives;

namespace Eternet.Web.Infrastructure.ServiceFabric;

public sealed class ReloadToken : IChangeToken
{
    private CancellationTokenSource _cts = new();

    public void OnReload()
    {
        var old = Interlocked.Exchange(ref _cts, new CancellationTokenSource());
        old.Cancel();
        old.Dispose();
    }

    public bool HasChanged => _cts.IsCancellationRequested;

    public bool ActiveChangeCallbacks => true;

    public IDisposable RegisterChangeCallback(Action<object?> callback, object? state) =>
        _cts.Token.Register(callback, state);
}
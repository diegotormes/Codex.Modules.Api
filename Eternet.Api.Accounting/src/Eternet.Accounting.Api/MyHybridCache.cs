namespace Eternet.Accounting.Api;

public class MyHybridCache : IHybridCache
{
    public string GetKey(string prefix, object[] parameters, string suffix = "")
    {
        return "";
    }

    public ValueTask<T> GetOrCreateAsync<TState, T>(string key, TState state, Func<TState, CancellationToken, ValueTask<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(default(T)!);
    }

    public ValueTask<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(default(T)!);
    }
}

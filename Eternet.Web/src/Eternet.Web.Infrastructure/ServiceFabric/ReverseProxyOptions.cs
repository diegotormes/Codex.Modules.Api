namespace Eternet.Web.Infrastructure.ServiceFabric;

public sealed class ReverseProxyOptions
{
    public int RefreshSeconds { get; init; } = 30;
    public List<ServiceDescriptor> Services { get; init; } = new();
}

public sealed class ServiceDescriptor
{
    public string RouteId { get; init; } = default!;
    public string ClusterId { get; init; } = default!;
    public string Path { get; init; } = default!;
    public string[] Methods { get; init; } = default!;
    public string ServiceUri { get; init; } = default!;
    public string EndpointName { get; init; } = default!;
    public bool Http2 { get; init; }
}

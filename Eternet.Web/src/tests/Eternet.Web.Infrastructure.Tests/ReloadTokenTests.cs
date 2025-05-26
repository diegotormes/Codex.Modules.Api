using Eternet.Web.Infrastructure.ServiceFabric;
using FluentAssertions;

namespace Eternet.Web.Infrastructure.Tests;

public class ReloadTokenTests
{
    [Fact]
    public void HasChanged_BeforeAndAfterOnReload_ReturnsFalse()
    {
        var token = new ReloadToken();
        token.HasChanged.Should().BeFalse();
        token.OnReload();
        token.HasChanged.Should().BeFalse();
    }

    [Fact]
    public void RegisterChangeCallback_InvokedOnReload()
    {
        var token = new ReloadToken();
        var called = false;
        token.RegisterChangeCallback(_ => called = true, null);
        token.OnReload();
        SpinWait.SpinUntil(() => called, TimeSpan.FromSeconds(1)).Should().BeTrue();
    }

    [Fact]
    public void ActiveChangeCallbacks_ReturnsTrue()
    {
        var token = new ReloadToken();
        token.ActiveChangeCallbacks.Should().BeTrue();
    }
}

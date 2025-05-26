using System.Reflection;
using Bunit;
using FluentAssertions;
using Eternet.Web.UI.Components.DataGrid.Models;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Eternet.Web.UI.Tests.Components.DataGrid;

public class PresetManagerTests : Bunit.TestContext
{
    [Fact]
    public async Task SaveAsync_InvokesCallbackAndClearsInput()
    {
        var configs = new List<EternetDataGridConfiguration>();
        string? savedName = null;
        Services.AddFluentUIComponents();
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = RenderComponent<PresetManager>(p => p
            .Add(pc => pc.Configurations, configs)
            .Add(pc => pc.OnSave, (Func<string, Task<bool>>)(n => { savedName = n; return Task.FromResult(true); }))
        );

        typeof(PresetManager).GetField("_newName", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(cut.Instance, "Cfg1");

        var method = typeof(PresetManager).GetMethod("SaveAsync", BindingFlags.Public | BindingFlags.Instance)!;
        await ((Task)method.Invoke(cut.Instance, null)!).ConfigureAwait(true);

        savedName.Should().Be("Cfg1");
        var value = (string)typeof(PresetManager).GetField("_newName", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(cut.Instance)!;
        value.Should().BeEmpty();
    }
}

using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Eternet.Web.UI.Tests.Components.TextField;

public class EternetTextFieldTests : Bunit.TestContext
{
    private class Model
    {
        public string? Value { get; set; }
    }

    [Fact]
    public async Task InputValueChangedAsync_OnlyDigits_StripsNonDigits()
    {
        var model = new Model();
        var editContext = new EditContext(model);
        Services.AddFluentUIComponents();
        JSInterop.Mode = JSRuntimeMode.Loose;
        var cut = RenderComponent<EternetTextField>(parameters => parameters
            .Add(p => p.Label, "Test")
            .Add(p => p.For, () => model.Value)
            .Add(p => p.Value, model.Value)
            .Add(p => p.OnlyDigits, true)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<string?>(this, v => model.Value = v))
            .AddCascadingValue(editContext)
        );

        await cut.Instance.InputValueChangedAsync("a1b2");

        model.Value.Should().Be("12");
    }

    [Fact]
    public async Task InputValueChangedAsync_SetsValueDirectly_WhenNotOnlyDigits()
    {
        var model = new Model();
        var editContext = new EditContext(model);
        Services.AddFluentUIComponents();
        JSInterop.Mode = JSRuntimeMode.Loose;
        var cut = RenderComponent<EternetTextField>(parameters => parameters
            .Add(p => p.Label, "Test")
            .Add(p => p.For, () => model.Value)
            .Add(p => p.Value, model.Value)
            .Add(p => p.OnlyDigits, false)
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<string?>(this, v => model.Value = v))
            .AddCascadingValue(editContext)
        );

        await cut.Instance.InputValueChangedAsync("abc");

        model.Value.Should().Be("abc");
    }
}

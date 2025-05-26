using System.Reflection;
using Bunit;
using FluentAssertions;
using Eternet.Web.UI.Components.DataGrid.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Eternet.Web.UI.Tests.Components.DataGrid;

public class EternetColumnSelectorDialogTests : Bunit.TestContext
{
    [Fact]
    public void HandleKeyDownAsync_KeyIsEscape_ClosesDialog()
    {
        var states = new List<EternetDataGridColumnState>
        {
            new() { ColumnName = "Id", Title = "Id", IsVisible = true }
        };
        var configs = new List<EternetDataGridConfiguration>();
        var visible = true;
        Services.AddFluentUIComponents();
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = RenderComponent<EternetColumnSelectorDialog>(p => p
            .Add(pc => pc.Visible, visible)
            .Add(pc => pc.VisibleChanged, EventCallback.Factory.Create<bool>(this, v => visible = v))
            .Add(pc => pc.CurrentColumnStates, states)
            .Add(pc => pc.SavedConfigurations, configs)
        );

        cut.Find("fluent-dialog").KeyDown(new KeyboardEventArgs { Key = "Escape" });

        visible.Should().BeFalse();
    }

    [Fact]
    public void HandleKeyDownAsync_KeyIsCtrlSAndOnPresets_SavesConfiguration()
    {
        var states = new List<EternetDataGridColumnState>
        {
            new() { ColumnName = "Id", Title = "Id", IsVisible = true }
        };
        var configs = new List<EternetDataGridConfiguration>();
        string? savedName = null;
        Services.AddFluentUIComponents();
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = RenderComponent<EternetColumnSelectorDialog>(p => p
            .Add(pc => pc.Visible, true)
            .Add(pc => pc.OnSaveConfiguration, (Func<string, Task<bool>>)(n => { savedName = n; return Task.FromResult(true); }))
            .Add(pc => pc.CurrentColumnStates, states)
            .Add(pc => pc.SavedConfigurations, configs)
        );

        var activeField = typeof(EternetColumnSelectorDialog)
            .GetField("_activeTab", BindingFlags.NonPublic | BindingFlags.Instance)!;
        activeField.SetValue(cut.Instance, "presets");

        var presetField = typeof(EternetColumnSelectorDialog)
            .GetField("_presetManager", BindingFlags.NonPublic | BindingFlags.Instance)!;
        var presetManager = (PresetManager)presetField.GetValue(cut.Instance)!;
        typeof(PresetManager)
            .GetField("_newName", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(presetManager, "Cfg1");

        cut.Find("fluent-dialog").KeyDown(new KeyboardEventArgs { Key = "s", CtrlKey = true });

        savedName.Should().Be("Cfg1");
    }

    [Fact]
    public void HandleKeyDownAsync_KeyIsAlt1_ChangesActiveTabToColumns()
    {
        var states = new List<EternetDataGridColumnState>
        {
            new() { ColumnName = "Id", Title = "Id", IsVisible = true }
        };
        var configs = new List<EternetDataGridConfiguration>();
        Services.AddFluentUIComponents();
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = RenderComponent<EternetColumnSelectorDialog>(p => p
            .Add(pc => pc.Visible, true)
            .Add(pc => pc.CurrentColumnStates, states)
            .Add(pc => pc.SavedConfigurations, configs)
        );

        var activeField = typeof(EternetColumnSelectorDialog)
            .GetField("_activeTab", BindingFlags.NonPublic | BindingFlags.Instance)!;
        activeField.SetValue(cut.Instance, "presets");

        cut.Find("fluent-dialog").KeyDown(new KeyboardEventArgs { Key = "1", AltKey = true });

        activeField.GetValue(cut.Instance).Should().Be("columns");
    }

    [Fact]
    public void HandleKeyDownAsync_KeyIsAlt2_ChangesActiveTabToPresetsAndClearsFilter()
    {
        var states = new List<EternetDataGridColumnState>
        {
            new() { ColumnName = "Id", Title = "Id", IsVisible = true }
        };
        var configs = new List<EternetDataGridConfiguration>();
        Services.AddFluentUIComponents();
        JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = RenderComponent<EternetColumnSelectorDialog>(p => p
            .Add(pc => pc.Visible, true)
            .Add(pc => pc.CurrentColumnStates, states)
            .Add(pc => pc.SavedConfigurations, configs)
        );

        var activeField = typeof(EternetColumnSelectorDialog)
            .GetField("_activeTab", BindingFlags.NonPublic | BindingFlags.Instance)!;
        activeField.SetValue(cut.Instance, "columns");

        var selectorField = typeof(EternetColumnSelectorDialog)
            .GetField("_columnSelector", BindingFlags.NonPublic | BindingFlags.Instance)!;
        var selector = (ColumnSelector)selectorField.GetValue(cut.Instance)!;
        typeof(ColumnSelector)
            .GetField("_filter", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(selector, "xyz");

        cut.Find("fluent-dialog").KeyDown(new KeyboardEventArgs { Key = "2", AltKey = true });

        activeField.GetValue(cut.Instance).Should().Be("presets");
        typeof(ColumnSelector)
            .GetField("_filter", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(selector)!
            .Should().Be(string.Empty);
    }
}

using Eternet.Web.UI.Components.DataGrid.Models;
using Microsoft.AspNetCore.Components.Web;

namespace Eternet.Web.UI;

public partial class EternetColumnSelectorDialog : ComponentBase
{
    [Parameter] public bool Visible { get; set; }
    [Parameter] public EventCallback<bool> VisibleChanged { get; set; }

    [Parameter, EditorRequired] public required List<EternetDataGridColumnState> CurrentColumnStates { get; set; }
    [Parameter] public EventCallback<List<EternetDataGridColumnState>> CurrentColumnStatesChanged { get; set; }

    [Parameter, EditorRequired] public required List<EternetDataGridConfiguration> SavedConfigurations { get; set; }

    [Parameter] public Func<string, Task<bool>>? OnSaveConfiguration { get; set; }
    [Parameter] public EventCallback<string> OnLoadConfiguration { get; set; }
    [Parameter] public EventCallback<string> OnDeleteConfiguration { get; set; }
    [Parameter] public EventCallback<(string Name, bool IsDefault)> OnSetDefaultConfiguration { get; set; }
    [Parameter] public EventCallback<List<EternetDataGridColumnState>> OnColumnStatesChanged { get; set; }

    private PresetManager? _presetManager;
    private ColumnSelector? _columnSelector;
    private string _activeTab = "columns";

    private bool HasVisibleColumns => CurrentColumnStates.Any(c => c.IsVisible);

    private async Task CommitAsync() => await CloseDialogAsync().ConfigureAwait(false);

    private async Task CloseDialogAsync() => await VisibleChanged.InvokeAsync(false).ConfigureAwait(false);

    private void OnTabChanged(string? newTabId)
    {
        if (_activeTab == "columns" && newTabId != "columns")
        {
            _columnSelector?.ClearFilter();
        }
        _activeTab = newTabId ?? "columns";
    }

    private async Task HandleKeyDownAsync(KeyboardEventArgs e)
    {
        if (e.Key == "Escape")
        {
            await CloseDialogAsync().ConfigureAwait(false);
        }
        else if ((e.Key == "s" || e.Key == "S") && e.CtrlKey && _activeTab == "presets" && _presetManager is not null)
        {
            await _presetManager.SaveAsync().ConfigureAwait(false);
        }
        else if (e.AltKey && (e.Key == "1" || e.Key == "2"))
        {
            var target = e.Key == "1" ? "columns" : "presets";
            OnTabChanged(target);
            StateHasChanged();
        }
    }
}

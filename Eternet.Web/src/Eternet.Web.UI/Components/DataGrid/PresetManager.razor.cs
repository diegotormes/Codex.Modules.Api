using Eternet.Web.UI.Components.DataGrid.Models;

namespace Eternet.Web.UI;

public partial class PresetManager(IToastService toastService) : ComponentBase
{
    [Parameter, EditorRequired] public required List<EternetDataGridConfiguration> Configurations { get; set; }
    [Parameter] public Func<string, Task<bool>>? OnSave { get; set; }
    [Parameter] public EventCallback<string> OnLoad { get; set; }
    [Parameter] public EventCallback<string> OnDelete { get; set; }
    [Parameter] public EventCallback<(string Name, bool IsDefault)> OnSetDefault { get; set; }

    private readonly IToastService _toastService = toastService;
    private FluentTextField? _configNameInput;
    private string _newName = string.Empty;
    private string? _configToDelete;
    private bool _showDeleteConfirm;

    private string? _defaultName => Configurations.FirstOrDefault(c => c.Default)?.Name;
    private IEnumerable<EternetDataGridConfiguration> OrderedConfigs => Configurations
        .OrderByDescending(c => c.Default)
        .ThenBy(c => c.Name);

    public async Task SaveAsync() => await SaveAsyncInternal().ConfigureAwait(false);

    private async Task SaveAsyncInternal()
    {
        if (string.IsNullOrWhiteSpace(_newName))
        {
            return;
        }

        var saved = OnSave is not null && await OnSave(_newName).ConfigureAwait(false);
        if (saved)
        {
            _toastService.ShowSuccess("Configuración guardada.");
            _newName = string.Empty;
        }
    }

    private async Task OnDefaultChangedAsync(string name, bool isDefault)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        await OnSetDefault.InvokeAsync((name, isDefault)).ConfigureAwait(false);

        if (isDefault)
        {
            _toastService.ShowSuccess($"'{name}' predeterminada.");
        }
    }

    private void ShowDeleteConfirm(string name)
    {
        _configToDelete = name;
        _showDeleteConfirm = true;
    }

    private void CloseDeleteConfirm()
    {
        _showDeleteConfirm = false;
        _configToDelete = null;
    }

    private async Task ConfirmDeleteAsync()
    {
        if (_configToDelete is not null)
        {
            await OnDelete.InvokeAsync(_configToDelete).ConfigureAwait(false);
            _toastService.ShowSuccess("Configuración eliminada.");
        }

        CloseDeleteConfirm();
    }
}

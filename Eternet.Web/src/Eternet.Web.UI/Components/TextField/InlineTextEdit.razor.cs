using Eternet.Web.UI.Abstractions.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;

namespace Eternet.Web.UI;

public partial class InlineTextEdit
{
    private bool _isEditing;
    private IServiceScope _scope;
    private readonly ISourcePropertiesService _sourcePropertiesService;
    private readonly IToastService _toastService;

    [Parameter][EditorRequired] public required string Value { get; set; }
    [Parameter][EditorRequired] public required object EntityId { get; set; }
    [Parameter][EditorRequired] public required PropertyInlineEditor Editor { get; set; }
    [Parameter] public EventCallback OnUpdate { get; set; }
    private string _value
    {
        get => Value;
        set
        {
            if (Value != value)
            {
                Value = value;
            }
        }
    }
    private int _maxLength = 255;

    private EternetButton? _buttonSave;

    public InlineTextEdit(
        IServiceProvider serviceProvider,
        ISourcePropertiesService sourcePropertiesService,
        IToastService toastService)
    {
        _scope = serviceProvider.CreateScope();
        _sourcePropertiesService = sourcePropertiesService;
        _toastService = toastService;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (Editor?.EntityType is not null && Editor?.PropertyName is not null)
        {
            var maxLength = _sourcePropertiesService.GetMaxLength(Editor.EntityType, Editor.PropertyName);
            if (maxLength is not null)
            {
                _maxLength = maxLength.Value;
            }
        }
    }

    private async ValueTask SaveAsync()
    {
        var capturedValue = _value;
        var viewService = _scope.ServiceProvider.GetRequiredService<IViewsService>();
        try
        {
            var resultSource = await viewService.UpdateSourcePropertyAsync(
                EntityId,
                Editor.EntityType,
                Editor.PropertyName,
                capturedValue).ConfigureAwait(false);
            if (resultSource)
            {
                var viewResult = await viewService.UpdateViewPropertyAsync(
                    EntityId,
                    Editor.ViewType,
                    Editor.ColumnName,
                    capturedValue).ConfigureAwait(false);

                if (viewResult)
                {
                    _toastService.ShowSuccess("Datos actualizados!");
                    await OnUpdate.InvokeAsync().ConfigureAwait(false);
                }
                else
                {
                    _toastService.ShowWarning("Datos actualizados en origen falló al actualizar en vista!");
                }
            }
            else
            {
                _toastService.ShowError("Error al actualizar en origen!");
            }
        }
        catch (Exception ex)
        {
            var error = ex.InnerException is null ? ex.Message : ex.InnerException.Message;
            _toastService.ShowError($"Error al actualizar en origen!<br>{error}");
        }
        _isEditing = false;
    }

    private async ValueTask HandleKey(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" && _buttonSave is not null)
        {
            await _buttonSave.Click().ConfigureAwait(false);
            return;
        }
        if (e.Key == "Escape")
        {
            _isEditing = false;
        }
    }

    private void EditClick()
    {
        _isEditing = true;
    }

}
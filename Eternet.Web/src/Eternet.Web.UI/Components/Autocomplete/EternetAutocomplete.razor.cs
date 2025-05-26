using System.Linq.Expressions;

namespace Eternet.Web.UI;

public partial class EternetAutocomplete<TItem>
{
    [Parameter, EditorRequired] public required string Label { get; set; }
    [Parameter, EditorRequired] public required Expression<Func<IEnumerable<TItem>>> For { get; set; }
    [Parameter, EditorRequired] public required Func<TItem, string> OptionText { get; set; }
    [Parameter, EditorRequired] public EventCallback<OptionsSearchEventArgs<TItem>> OnOptionsSearch { get; set; }
    [Parameter, EditorRequired] public required IEnumerable<TItem> Value { get; set; }
    [Parameter] public EventCallback<IEnumerable<TItem>> ValueChanged { get; set; }
    [Parameter] public int? MaximumSelectedOptions { get; set; }
    [Parameter] public bool Immediate { get; set; } = true;
    //TODO: see if with later FluentUI versions it works, actually int 4.10.4 it doesn't. When required=true even if the field is complete, it shows a required message.
    [Parameter] public bool Required { get; set; } = false;
    [Parameter] public bool FullWidth { get; set; } = false;

    private string GetClass()
    {
        return FullWidth ? "w-100" : string.Empty;
    }

    private async Task HandleSelectedOptionsChange(IEnumerable<TItem> newValue)
    {
        Value = newValue;
        await ValueChanged.InvokeAsync(Value).ConfigureAwait(false);
        await InvokeAsync(StateHasChanged).ConfigureAwait(false);
    }
}

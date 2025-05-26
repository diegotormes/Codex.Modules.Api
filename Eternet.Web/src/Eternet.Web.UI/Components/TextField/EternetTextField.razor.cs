using System.Linq.Expressions;
using Eternet.Web.UI.Extensions;
using Microsoft.AspNetCore.Components.Forms;

namespace Eternet.Web.UI;

public partial class EternetTextField
{
    [Parameter, EditorRequired] public required string Label { get; set; }
    [Parameter, EditorRequired] public required Expression<Func<string?>> For { get; set; }
    [Parameter, EditorRequired] public string? Value { get; set; }
    [Parameter] public EventCallback<string?> ValueChanged { get; set; }
    [Parameter] public bool Immediate { get; set; } = true;
    [Parameter] public bool Required { get; set; } = false;
    [Parameter] public bool FullWidth { get; set; } = false;
    [Parameter] public bool OnlyDigits { get; set; } = false;

    [CascadingParameter] public EditContext? EditContext { get; set; }

    public async Task InputValueChangedAsync(string? value)
    {
        if (OnlyDigits)
        {
            Value = value.GetNumericValue();
        }
        else
        {
            Value = value;
        }
        await ValueChanged.InvokeAsync(Value).ConfigureAwait(false);
        await InvokeAsync(() => EditContext?.NotifyFieldChanged(FieldIdentifier.Create(For))).ConfigureAwait(false);
    }

    private string GetClass()
    {
        return FullWidth ? "w-100" : string.Empty;
    }
}

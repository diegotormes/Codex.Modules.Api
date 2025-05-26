namespace Eternet.Web.UI;

public partial class EternetDataGridColumnBooleanFilter : ComponentBase
{
    [Parameter] public string Label { get; set; } = "Boolean Filter";
    [Parameter] public bool? FilterValue { get; set; }
    [Parameter] public EventCallback<bool?> FilterValueChanged { get; set; }

    private string? SelectedValue
    {
        get => FilterValue?.ToString();
        set => OnValueChanged(value);
    }

    private void OnValueChanged(string? newValue)
    {
        bool? parsed = newValue switch
        {
            "True" => true,
            "False" => false,
            _ => null
        };

        if (FilterValue != parsed)
        {
            FilterValue = parsed;
            FilterValueChanged.InvokeAsync(FilterValue);
        }
    }
}

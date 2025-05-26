namespace Eternet.Web.UI;

public partial class EternetDataGridColumnEnumFilter<EnumType> : ComponentBase
    where EnumType : struct, Enum
{
    [Parameter] public string Label { get; set; } = "Enum Filter";
    [Parameter] public EnumType? SelectedValue { get; set; }
    [Parameter] public EventCallback<EnumType?> SelectedValueChanged { get; set; }

    // Use property wrapper for binding
    private string? CurrentValueAsString
    {
        get => SelectedValue?.ToString();
        set => OnValueChanged(value);
    }

    private void OnValueChanged(string? newVal)
    {
        EnumType? parsed = null;
        if (!string.IsNullOrWhiteSpace(newVal) && Enum.TryParse<EnumType>(newVal, out var e))
        {
            parsed = e;
        }

        if (SelectedValue?.Equals(parsed) != true) // Check if value actually changed
        {
            SelectedValue = parsed;
            SelectedValueChanged.InvokeAsync(SelectedValue);
        }
    }

    // Helper property to get enum values for the select list
    protected IEnumerable<EnumType> EnumValues => Enum.GetValues(typeof(EnumType)).Cast<EnumType>();
}
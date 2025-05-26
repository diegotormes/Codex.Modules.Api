namespace Eternet.Web.UI;

public partial class EternetDataGridColumnNumberFilter : ComponentBase
{
    [Parameter] public string Label { get; set; } = "Number Filter";
    [Parameter] public NumericFilter? FilterValue { get; set; }
    [Parameter] public EventCallback<NumericFilter?> FilterValueChanged { get; set; }

    private double? CurrentNumberValue
    {
        get => FilterValue?.Number;
        set
        {
            if (FilterValue?.Number != value)
            {
                FilterValue = (FilterValue ?? new NumericFilter()) with { Number = value };
                TriggerFilterChanged();
            }
        }
    }

    private FilterNumericOperator CurrentOperator
    {
        get => FilterValue?.Operator ?? FilterNumericOperator.GreaterOrEqual;
        set
        {
            if (FilterValue?.Operator != value)
            {
                FilterValue = (FilterValue ?? new NumericFilter()) with { Operator = value };
                TriggerFilterChanged();
            }
        }
    }

    private void TriggerFilterChanged()
    {
        FilterValueChanged.InvokeAsync(FilterValue);
    }
}

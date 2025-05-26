namespace Eternet.Web.UI;

public partial class EternetDataGridColumnDateFilter : ComponentBase
{
    [Parameter] public string Label { get; set; } = "Date Filter";
    [Parameter] public DateTime? FilterDate { get; set; }
    [Parameter] public EventCallback<DateTime?> FilterDateChanged { get; set; }

    // Use property wrapper for binding to avoid potential issues with direct parameter binding in complex scenarios
    private DateTime? CurrentValue
    {
        get => FilterDate;
        set
        {
            if (FilterDate != value)
            {
                FilterDate = value;
                FilterDateChanged.InvokeAsync(FilterDate);
            }
        }
    }
}
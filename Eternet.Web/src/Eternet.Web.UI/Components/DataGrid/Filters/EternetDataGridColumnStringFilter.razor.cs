using Microsoft.AspNetCore.Components.Web;

namespace Eternet.Web.UI;

public partial class EternetDataGridColumnStringFilter : ComponentBase
{
    [Parameter] public string Label { get; set; } = "Filtro Avanzado";
    [Parameter, EditorRequired] required public StringFilters? FilterValue { get; set; }
    [Parameter] public EventCallback<StringFilters?> FilterValueChanged { get; set; }

    private CombineOperator selectedCombineOperator = CombineOperator.Or;

    private List<FilterStringOperator> filterOperatorOptions = Enum.GetValues<FilterStringOperator>().ToList();
    private List<CombineOperator> combineOperatorOptions = Enum.GetValues<CombineOperator>().ToList();

    private async ValueTask AddCondition()
    {
        if (FilterValue is null)
        {
            FilterValue = new StringFilters();
            FilterValue.Conditions.Add(new FilterCondition());
            await FilterValueChanged.InvokeAsync(FilterValue).ConfigureAwait(false);
        }
        else
        {
            FilterValue.Conditions.Add(new FilterCondition());
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (FilterValue is null)
            {
                await AddCondition().ConfigureAwait(false);
            }
            StateHasChanged();
        }
    }

    private async ValueTask OnKeyDown(KeyboardEventArgs args)
    {
        if (args is not null &&
            args.CtrlKey &&
            args.Key == "Enter")
        {
            await ApplyFilter().ConfigureAwait(false);
            return;
        }
    }

    private void RemoveCondition(Guid id)
    {
        var conditionToRemove = FilterValue?.Conditions.FirstOrDefault(c => c.Id == id);
        if (conditionToRemove != null)
        {
            FilterValue?.Conditions.Remove(conditionToRemove);
            if (FilterValue?.Conditions.Count <= 1)
            {
                selectedCombineOperator = CombineOperator.And;
            }
            StateHasChanged();
        }
    }

    private async ValueTask ApplyFilter()
    {
        if (FilterValue is null)
        {
            return;
        }
        if (FilterValue.Conditions.Any(c => string.IsNullOrWhiteSpace(c.Value)))
        {
            var conditions = FilterValue.Conditions.
                Where(c => string.IsNullOrWhiteSpace(c.Value)).
                ToList();
            foreach (var condition in conditions)
            {
                FilterValue.Conditions.Remove(condition);
            }
        }

        if (FilterValue.IsActive)
        {
            await FilterValueChanged.InvokeAsync(FilterValue).ConfigureAwait(false);
        }
        else
        {
            await FilterValueChanged.InvokeAsync(null).ConfigureAwait(false);
        }
    }

    private async Task ClearFilter()
    {
        FilterValue?.Conditions.Clear();
        selectedCombineOperator = CombineOperator.Or;
        await FilterValueChanged.InvokeAsync(null).ConfigureAwait(false);
        StateHasChanged();
    }

    public async Task ResetFilterAsync()
    {
        await ClearFilter().ConfigureAwait(false);
    }
}
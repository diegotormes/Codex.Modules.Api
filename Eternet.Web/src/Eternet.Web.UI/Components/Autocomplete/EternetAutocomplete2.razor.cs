using System.Linq.Expressions;
using System.Reflection;
using Eternet.Web.UI.Abstractions.Services;
using Eternet.Web.UI.Models;

namespace Eternet.Web.UI;

public partial class EternetAutocomplete2<TItem, TOption> where TItem : class
{
    [Inject] public required IQueryService<TItem> QueryService { get; set; }
    [Parameter, EditorRequired] public required string Label { get; set; }
    [Parameter, EditorRequired] public required Expression<Func<IEnumerable<TOption>>> For { get; set; }
    [Parameter, EditorRequired] public required Expression<Func<TItem, TOption>> OptionSelector { get; set; }
    [Parameter, EditorRequired] public required Expression<Func<TOption, string>> OptionText { get; set; }
    [Parameter, EditorRequired] public required Expression<Func<TItem, string>> FilterText { get; set; }
    [Parameter, EditorRequired] public required IEnumerable<TOption> Value { get; set; }
    [Parameter] public SearchType SearchType { get; set; } = SearchType.StartsWith;
    [Parameter] public SearchOption SearchOption { get; set; } = SearchOption.CaseInsensitive;
    [Parameter] public EventCallback<IEnumerable<TOption>> ValueChanged { get; set; }
    [Parameter] public int? MaximumSelectedOptions { get; set; }
    [Parameter] public int MaximumItems { get; set; } = 10;
    [Parameter] public bool Immediate { get; set; } = false;
    [Parameter] public int ImmediateDelay { get; set; } = 500;

    //TODO: see if with later FluentUI versions it works, actually int 4.10.4 it doesn't. When required=true even if the field is complete, it shows a required message.
    [Parameter] public bool Required { get; set; } = false;
    [Parameter] public bool FullWidth { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync().ConfigureAwait(false);
    }

    private string GetClass()
    {
        return FullWidth ? "w-100" : string.Empty;
    }

    private Func<TOption, string> GetOptionText()
    {
        return OptionText.Compile();
    }

    private async Task HandleSelectedOptionsChange(IEnumerable<TOption> newValue)
    {
        Value = newValue;
        await ValueChanged.InvokeAsync(Value).ConfigureAwait(false);
        await InvokeAsync(StateHasChanged).ConfigureAwait(false);
    }

    private static Expression<Func<TItem, bool>> BuildSearchExpression(
        Expression<Func<TItem, string>> selector,
        SearchType searchType,
        SearchOption searchOption,
        string searchText)
    {
        var parameter = selector.Parameters[0];
        var propertyExpression = selector.Body;

        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
        var lowerProperty = Expression.Call(propertyExpression, toLowerMethod!);

        var searchExpression = searchOption == SearchOption.CaseInsensitive ? Expression.Constant(searchText.ToLower()) : Expression.Constant(searchText);

        MethodInfo searchMethod;
        if (searchType == SearchType.Contains)
        {
            searchMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!;
        }
        else
        {
            searchMethod = typeof(string).GetMethod(nameof(string.StartsWith), [typeof(string)])!;
        }

        if (searchOption == SearchOption.CaseInsensitive)
        {
            var lowerSearchText = Expression.Constant(searchText.ToLower());
            var containsCall = Expression.Call(lowerProperty, searchMethod, lowerSearchText);
            return Expression.Lambda<Func<TItem, bool>>(containsCall, parameter);
        }
        else
        {
            var containsExpression = Expression.Call(propertyExpression, searchMethod, searchExpression);
            return Expression.Lambda<Func<TItem, bool>>(containsExpression, parameter);
        }
    }

    private async Task LoadItems(OptionsSearchEventArgs<TOption> args)
    {
        string searchText = args.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            args.Items = [];
            return;
        }

        var queryable = QueryService.GetQuery();
        var predicate = BuildSearchExpression(FilterText, SearchType, SearchOption, searchText);

        var projectedQuery = queryable
            .Where(predicate)
            .OrderBy(FilterText)
            .Take(MaximumItems)
            .Select(OptionSelector);

        var items = await QueryService.ToListAsync(projectedQuery).ConfigureAwait(false);

        args.Items = items;
    }

}

public class ParameterReplacer : ExpressionVisitor
{
    private readonly ParameterExpression _oldParameter;
    private readonly Expression _newExpression;

    public ParameterReplacer(ParameterExpression oldParameter, Expression newExpression)
    {
        _oldParameter = oldParameter;
        _newExpression = newExpression;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == _oldParameter ? _newExpression : base.VisitParameter(node);
    }
}


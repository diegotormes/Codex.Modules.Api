namespace Eternet.Web.UI;

public class StringFilters
{
    public CombineOperator CombineOperator { get; set; } = CombineOperator.Or;
    public List<FilterCondition> Conditions { get; set; } = [];
    public bool IsActive => Conditions.Count > 0 && Conditions.Any(c => !string.IsNullOrWhiteSpace(c.Value));
}

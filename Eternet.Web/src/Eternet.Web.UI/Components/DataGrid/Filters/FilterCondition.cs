namespace Eternet.Web.UI;

public class FilterCondition
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public FilterStringOperator Operator { get; set; } = FilterStringOperator.Contains;
    public string? Value { get; set; }
    public bool CaseInsensitive { get; set; }
}

namespace Eternet.Web.UI;

public record NumericFilter(double? Number = null, FilterNumericOperator Operator = FilterNumericOperator.GreaterOrEqual);

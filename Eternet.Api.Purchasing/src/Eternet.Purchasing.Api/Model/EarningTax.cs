namespace Eternet.Purchasing.Api.Model;

public class EarningTax
{
    public required int Id { get; set; }
    public required string Description { get; set; }
    public required float RetentionPercentage { get; set; }
    public required float TaxableMinimum { get; set; }
    public required float MinimumRetention { get; set; }
}

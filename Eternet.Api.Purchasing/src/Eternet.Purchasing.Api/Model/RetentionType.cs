namespace Eternet.Purchasing.Api.Model;

public class RetentionType
{
    public int Key { get; set; }

    public string Description { get; set; } = "";

    public int? AccountId { get; set; }

    public float MinimumTaxable { get; set; } = 0.0f;

    public short WithDetail { get; set; } = 0;
}

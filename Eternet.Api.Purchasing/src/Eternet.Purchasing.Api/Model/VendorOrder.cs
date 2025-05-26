namespace Eternet.Purchasing.Api.Model;

public class VendorOrder
{
    public int Key { get; set; }
    public DateTime OrderDate { get; set; }
    public int Buyer { get; set; }
    public string Transport { get; set; } = "";
    public DateTime EstimatedEntryDate { get; set; }
    public int State { get; set; }
    public int DeliverTo { get; set; }
    public string Observations { get; set; } = "";
    public int ProviderNumber { get; set; }
    public string Provider { get; set; } = "";
    public string ProviderOrderNumber { get; set; } = "";
    public DateTime? DeliveryDate { get; set; }
    public int IsRma { get; set; }
    public float? Dollar { get; set; }
    public float? TotalGoods { get; set; }
    public float? Expenses { get; set; }
    public float? TotalOrder { get; set; }
    public short Paid { get; set; }
    public int? InvoiceId { get; set; }
    public int? BudgetItemId { get; set; }
    public string AuthorizedBy { get; set; } = "";
}

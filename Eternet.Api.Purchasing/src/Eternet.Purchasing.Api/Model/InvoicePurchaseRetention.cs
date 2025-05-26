namespace Eternet.Purchasing.Api.Model;

public class InvoicePurchaseRetention
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int RetentionType { get; set; }
    public int? ProvinceId { get; set; }
    public decimal TotalAmount { get; set; }

    public ICollection<PurchaseRetention> Retentions { get; set; } = null!;

    public Invoice PurchaseInvoice { get; set; } = null!;

    public Province Province { get; set; } = null!;

}

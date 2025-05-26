namespace Eternet.Purchasing.Api.Model;

public class PurchaseRetention
{
    public int Id { get; set; }
    public int VatPurchaseId { get; set; }
    public string? RetentionCode { get; set; }
    public string? CertificateNumber { get; set; }
    public short? Month { get; set; }
    public short? Year { get; set; }
    public int? RetentionType { get; set; }
    public int? PurchaseInvoiceRetentionId { get; set; } 
    public VatPurchase VatPurchase { get; set; } = null!;
    public InvoicePurchaseRetention InvoicePurchaseRetention { get; set; } = null!;
    public RetentionType RetentionTypeReference { get; set; } = null!;

}

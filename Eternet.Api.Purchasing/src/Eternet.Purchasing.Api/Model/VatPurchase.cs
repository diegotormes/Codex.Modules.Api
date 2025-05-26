namespace Eternet.Purchasing.Api.Model;

public class VatPurchase
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int SalesPoint { get; set; }
    public string VoucherType { get; set; } = "";
    public string InvoiceType { get; set; } = "";
    public string BranchNumber { get; set; } = "";
    public string VoucherNumber { get; set; } = "";
    public DateTime? VatTaxDate { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string VendorName { get; set; } = "";
    public string Cuit { get; set; } = "";
    public string VatSituation { get; set; } = "";
    public decimal VatRate { get; set; }
    public decimal Taxable { get; set; }
    public decimal NonTaxable { get; set; }
    public decimal Exempt { get; set; }
    public decimal VatTotalAmount { get; set; }
    public decimal VatRetentionTotalAmount { get; set; }
    public decimal IbRetentionTotalAmount { get; set; }
    public decimal ProfitRetentionTotalAmount { get; set; }
    public decimal VatPerceptionTotalAmount { get; set; }
    public decimal IbPerceptionTotalAmount { get; set; }
    public decimal PurchasesC { get; set; }
    public decimal TotalAmount { get; set; }
    public int? CreditCardLiquidation { get; set; } = null;
    public decimal ProfitPerception { get; set; }
    public decimal VatRetentionPerception { get; set; }

    public ICollection<PurchaseRetention> PurchaseRetentions { get; set; } = null!;

    public Invoice PurchaseInvoice { get; set; } = null!;
}

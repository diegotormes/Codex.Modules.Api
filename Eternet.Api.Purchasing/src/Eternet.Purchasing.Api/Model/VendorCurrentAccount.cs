namespace Eternet.Purchasing.Api.Model;

public class VendorCurrentAccount
{
    public int Id { get; set; }
    public int Provider { get; set; }
    public DateTime Date { get; set; }
    public string InvoiceType { get; set; } = "";
    public string VoucherType { get; set; } = "";
    public string BranchNumber { get; set; } = "";
    public string VoucherNumber { get; set; } = "";
    public float? Debit { get; set; }
    public decimal? Credit { get; set; }
    public float? Balance { get; set; }
    public string Observations { get; set; } = "";
    public int? InvoiceId { get; set; }
    public int? PointOfSale { get; set; }
    public float AdvancePayment { get; set; }
    public string User { get; set; } = "";
    public short Security { get; set; }
    public Invoice? PurchaseInvoice { get; set; }
}

namespace Eternet.Purchasing.Api.Model;

public class Expense
{
    public int Key { get; set; }
    public int ServiceType { get; set; }
    public int ExpenseType { get; set; }
    public DateTime? Date { get; set; }
    public string Observations { get; set; } = "";
    public int? Provider { get; set; }
    public float? Amount { get; set; }
    public string Paid { get; set; } = "";
    public DateTime? DueDate { get; set; }
    public string Receipt { get; set; } = "";
    public int? PointOfSale { get; set; }
    public string Branch { get; set; } = "";
    public DateTime? VatDate { get; set; }
    public float? NonTaxable { get; set; }
    public float? Exempt { get; set; }
    public float? VatAmount { get; set; }
    public float? VatRetention { get; set; }
    public float? IBRetention { get; set; }
    public float? EarningsRetention { get; set; }
    public float? VatPerception { get; set; }
    public float? IBPerception { get; set; }
    public float? Total { get; set; }
    public string VoucherType { get; set; } = "";
    public string InvoiceType { get; set; } = "";
    public string ProviderName { get; set; } = "";
    public int? InvoiceId { get; set; }
    public short IsFictitious { get; set; }
    public int? PurchaseInvoiceDetailId { get; set; }
}

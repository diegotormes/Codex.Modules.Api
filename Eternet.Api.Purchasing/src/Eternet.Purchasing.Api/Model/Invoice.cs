using Eternet.Models.Abstractions;
using Eternet.Purchasing.Api.Extensions;

namespace Eternet.Purchasing.Api.Model;
public enum RetentionTypes
{
    RetentionVat = 1,
    RetentionGrossReceipts = 2,
    RetentionProfit = 3,
    RetentionSUSS = 4,
    RetentionCreditCardSales = 5
}

public enum PerceptionTypes
{
    PerceptionVat = 6,
    GrossReceipts = 7,
    PerceptionProfit = 8
}

public class Invoice : IIntIdentity
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public string InvoiceType { get; set; } = "";
    public string VoucherType { get; set; } = "";
    public string BranchNumber { get; set; } = "";
    public string VoucherNumber { get; set; } = "";
    public int? VendorId { get; set; }
    public int PaymentMethodId { get; set; }
    public DateTime IssueDate { get; set; }
    public string VendorName { get; set; } = "";
    public string VendorPostalAddress { get; set; } = "";
    public string VendorTaxId { get; set; } = "";
    public string VatStatus { get; set; } = "";
    public decimal? TotalAmount { get; set; }
    public decimal? Subtotal { get; set; }
    public decimal? VatTotalAmount { get; set; }
    public decimal? NetAmount { get; set; }
    public float? Discount { get; set; }
    public float? Surcharge { get; set; }
    public decimal? ExemptAmount { get; set; }
    public decimal? NonTaxableAmount { get; set; }
    public decimal? VatRetentionTotalAmount { get; set; }
    public decimal? VatPerceptionTotalAmount { get; set; }
    public decimal? IbRetentionTotalAmount { get; set; }
    public decimal? IbPerceptionTotalAmount { get; set; }
    public decimal? ProfitRetentionTotalAmount { get; set; }
    public decimal? ProfitPerceptionTotalAmount { get; set; } = 0;
    public decimal? TotalReference { get; set; }
    public decimal? TotalPayment { get; set; }
    public float? AmountToPay { get; set; } = 0;
    public DateTime? DueDate { get; set; }
    public DateTime? VatTaxDate { get; set; }
    public short? SalesPoint { get; set; }

    private string _description = "";
    public string Description
    {
        get => _description;
        set => _description = value.GetFromMaxLen(PurchaseInvoiceConstraints.DescriptionMaxLen);
    }

    public string Barcode { get; set; } = "";
    public short? PayAll { get; set; } = 1;
    public decimal? ExchangeRate { get; set; }
    public int? ServiceContractId { get; set; }
    public short? Month { get; set; }
    public short? Year { get; set; }

    private string _sharePointFile = "";
    public string SharePointFile
    {
        get => _sharePointFile;
        set
        {
            var escapedValue = Uri.EscapeDataString(value);
            if (escapedValue.Length > PurchaseInvoiceConstraints.SharePointFileMaxLen)
            {
                _sharePointFile = $"FileName_Too_Long_Max{PurchaseInvoiceConstraints.SharePointFileMaxLen}_Chars";
            }
            else
            {
                _sharePointFile = escapedValue;
            }
        }
    }

    public VendorServiceContract VendorServiceContract { get; set; } = null!;
    public CommercialRelationship CommercialRelationship { get; set; } = null!;

    public ICollection<InvoiceDetail> Details { get; set; } = null!;
    public ICollection<InvoicePurchaseRetention> PurchaseRetentions { get; set; } = null!;
    public ICollection<PurchasePaymentMethod> PurchasePaymentMethods { get; set; } = null!;
    public ICollection<VendorCurrentAccount> VendorCurrentAccounts { get; set; } = [];    
    //After commit this AggregateRoot
    public ICollection<VatPurchase> VatPurchases { get; set; } = null!;
}

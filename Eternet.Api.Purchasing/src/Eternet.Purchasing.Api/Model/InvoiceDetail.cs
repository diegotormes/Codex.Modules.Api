using Eternet.Purchasing.Api.Extensions;

namespace Eternet.Purchasing.Api.Model;

public static class PurchaseInvoiceDetailFields
{
    public static int DescriptionMaxLen { get; } = 45;
    public static int ServiceArticleIdMaxLen { get; } = 15;
}

public class InvoiceDetail
{
    private string _description = "";
    private string _serviceArticleId = "";

    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public string ServiceArticleId
    {
        get => _serviceArticleId;
        set => _serviceArticleId = value.GetFromMaxLen(PurchaseInvoiceDetailFields.ServiceArticleIdMaxLen);
    }
    public string Description
    {
        get => _description;
        set => _description = value.GetFromMaxLen(PurchaseInvoiceDetailFields.DescriptionMaxLen);
    }
    public float Quantity { get; set; }
    public decimal VatRate { get; set; }

    public decimal? Taxable { get; set; }
    public decimal? Subtotal { get; set; }
    public decimal? Vat { get; set; }
    public decimal? NonTaxable { get; set; }
    public decimal? Exempt { get; set; }
    public decimal? TotalDetail { get; set; }

    public float AmountToPay { get; set; } = 0;
    public int PaysAll { get; set; } = 1;
    public float Paid { get; set; } = 0;
    public short ExpenseArticle { get; set; }
    public int? ArticleId { get; set; } = -1;
    public int? ServiceId { get; set; } = -1;
    public int? OrderDetailId { get; set; }
    public decimal Retentions { get; set; } = 0;
    public float? AccountableNet { get; set; }
    public Invoice PurchaseInvoice { get; set; } = null!;
}
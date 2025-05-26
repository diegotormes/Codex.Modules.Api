namespace Eternet.Purchasing.Api.Model;

public class PurchasePaymentMethod
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public string Description { get; set; } = "";
    public DateTime? Date { get; set; }
    public decimal TotalAmount { get; set; }
    public int PaymentMethodId { get; set; }

    public PaymentMethod PaymentMethod { get; set; } = null!;

    public Invoice PurchaseInvoice { get; set; } = null!;

}

namespace Eternet.Purchasing.Api.Model;

public class PaymentMethod
{
    public int Id { get; set; }
    public string Description { get; set; } = "";

    public ICollection<PurchasePaymentMethod> PurchaseInvoicesPayments { get; set; } = null!;
}

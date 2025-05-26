namespace Eternet.Purchasing.Api.Model;

public class Province
{
    public int Id { get; set; }
    public string Description { get; set; } = "";
    public ICollection<InvoicePurchaseRetention> InvoicePurchaseRetention { get; set; } = null!;
}

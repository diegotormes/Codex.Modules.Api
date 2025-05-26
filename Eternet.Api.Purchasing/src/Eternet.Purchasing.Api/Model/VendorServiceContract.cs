namespace Eternet.Purchasing.Api.Model;

public class VendorServiceContract
{
    public int Id { get; set; }
    public int VendorId { get; set; }
    public int ExpenseId { get; set; }
    public string Description { get; set; } = "";
    public short Months { get; set; }
    public DateTime RegistrationDate { get; set; }

    public ExpenseType ExpenseType { get; set; } = null!;

    public ICollection<Invoice> PurchaseInvoices { get; set; } = null!;
}

namespace Eternet.Purchasing.Api.Model;

public class VendorExpenseType
{
    public int Id { get; set; }
    public int VendorId { get; set; }
    public CommercialRelationship Vendor { get; set; } = default!;
    public int ExpenseTypeId { get; set; }
    public ExpenseType ExpenseType { get; set; } = default!;
}
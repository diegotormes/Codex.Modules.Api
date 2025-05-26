using Eternet.Models.Abstractions;

namespace Eternet.Purchasing.Api.Model;

public record ExpenseType : IIntIdentity
{
    public int Id { get; set; }
    public string Description { get; set; } = "";
    public bool IsActive { get; set; }
    public int? AccountId { get; set; }
    //public AccountingChart? Account { get; set; }
    public float VatRate { get; set; } = 21.0f;
    public int EarningTaxId { get; set; }
    public EarningTax EarningTax { get; set; } = default!;
    public short IncludeInPurchases { get; set; } = 1;
    public string KeyString { get; set; } = default!;

    public ICollection<VendorServiceContract> ProviderServiceContracts { get; set; } = null!;
    
}

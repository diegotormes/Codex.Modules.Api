namespace Eternet.Purchasing.Api.Model;

public record CommercialRelationship
{
    public int Id { get; set; }
    public string DocumentType { get; set; } = "";
    public string? DocumentNumber { get; set; }
    public string SurnameAndName { get; set; } = "";
    public string? PhoneNumber { get; set; }
    public string VatStatus { get; set; } = "";
    public string? VendorTaxId { get; set; }
    public short SalesPoint { get; set; }
    public string RelationshipType { get; set; } = "Cliente";
    public string? Observations { get; set; }
    public string MonthlyBilling { get; set; } = "No";
    public string? PostalAddress { get; set; }
    public string? PaymentAddress { get; set; }
    public string? PostalCode { get; set; }
    public string? CityDescription { get; set; }
    public float Balance { get; set; } = 0;
    public string Uncollectible { get; set; } = "No";
    public string Debit { get; set; } = "No";
    public short? Area { get; set; } = 0;
    public int? Category { get; set; } = 1;
    public float ProvBalance { get; set; } = 0;
    public int? Street { get; set; }
    public int? CityId { get; set; }
    public short? StreetNumber { get; set; }
    public int? PaymentStreet { get; set; }
    public int? PaymentStreetHeight { get; set; }
    public string? PhoneAreaCode { get; set; }
    public int BaproCouponNumber { get; set; } = 1;
    public string? PaymentPostalCode { get; set; }
    public short HasTelephony { get; set; } = 0;
    public short DebitType { get; set; } = 0;
    public string? CardholderName { get; set; }
    public short? DebtMessage { get; set; } = 1;
    public int Neighborhood { get; set; } = 0;
    public int? DeliveryCity { get; set; }
    public short? PrintedBill { get; set; } = 0;
    public string? SmsCellphone { get; set; }
    public short DoesNotInformMail { get; set; } = 0;
    public short InvalidMail { get; set; } = 0;
    public int? AccountingChartId { get; set; }
    public string? FirstName { get; set; }
    public short AppliesEarningsTax { get; set; } = 1;
    public short IvaBook { get; set; } = 1;
    public short HighValue { get; set; } = 0;
    public short DgoActive { get; set; } = 0;
    public string? LastDgoUser { get; set; }
    public DateTime? LastDgoDate { get; set; }
    public int? PaymentMethodId { get; set; }
    public short? PaymentTermsInDays { get; set; }

    public VendorBankAccount? VendorBankAccount { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    //public AccountingChart? AccountingChart { get; set; }
    public City? City { get; set; }
    public ICollection<Invoice> PurchaseInvoices { get; set; } = [];
}
namespace Eternet.Purchasing.Api.Model;

public record VendorBankAccount
{
    public int Id { get; set; }
    public int CommercialRelationshipId { get; set; }
    public CommercialRelationship CommercialRelationship { get; set; } = default!;
    public required bool Local { get; set; }
    public int? Type { get; set; }
    public int? Branch { get; set; }
    public string? Number { get; set; }
    public string? CbuBlockOne { get; set; }
    public string? CbuBlockTwo { get; set; }
    public int? DniType { get; set; }
    public string? Beneficiary { get; set; }
    public string? Email { get; set; }
    public string? Dni { get; set; }
    public bool Enabled { get; set; } = true;
}

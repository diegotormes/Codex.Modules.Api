namespace Eternet.Accounting.Contracts.VatClosures.Responses;

public record VatDebitResponse
{
    public int Id { get; init; }
    public required string Description { get; init; }
    public required string TaxResponsibility { get; init; }
    public decimal VatRate { get; init; }
    public decimal TaxableAmount { get; init; }
    public decimal VatAmount { get; init; }
    public decimal Total { get; init; }
}

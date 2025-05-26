namespace Eternet.Accounting.Contracts.VatClosures.Responses;

public record VatRetentionResponse
{
    public int Id { get; init; }
    public required DateOnly Date { get; init; }
    public required string RetentionType { get; init; }
    public decimal Amount { get; init; }
    public required string Name { get; init; }
    public required string TaxNumber { get; init; }
    public required string RetentionCode { get; init; }
    public string? CertificateNumber { get; init; }
}

namespace Eternet.Accounting.Contracts.VatClosures.Responses;

public record VatSalaryResponse
{
    public int Id { get; init; }
    public short Month { get; init; }
    public short Year { get; init; }
    public required string Description { get; init; }
    public decimal Total { get; init; }
    public decimal IvaCf { get; init; }
}

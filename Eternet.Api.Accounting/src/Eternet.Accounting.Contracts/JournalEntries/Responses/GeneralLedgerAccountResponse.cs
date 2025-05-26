namespace Eternet.Accounting.Contracts.JournalEntries.Responses;

public record GeneralLedgerAccountResponse
{
    public required int Id { get; init; }
    public required short Order { get; init; }
    public required string Description { get; init; }
    public string? Notes { get; init; }
    public int? ParentLedgerAccountId { get; init; }
    public int? AccountingPeriodId { get; init; }
    public required string NormalBalance { get; init; }
    public required string LedgerAccountType { get; init; }
}

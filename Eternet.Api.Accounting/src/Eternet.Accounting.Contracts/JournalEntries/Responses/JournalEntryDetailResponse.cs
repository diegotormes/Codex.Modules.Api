namespace Eternet.Accounting.Contracts.JournalEntries.Responses;

public record JournalEntryDetailResponse
{
    public required int GeneralLedgerAccountId { get; init; }
    public required double? Debit { get; init; }
    public required double? Credit { get; init; }
}

public enum JournalEntryPeriodStatus
{
    Pending,
    Current
}
namespace Eternet.Accounting.Contracts.JournalEntries.Responses;

public record JournalEntryResponse
{
    public Guid Id { get; init; }
    public required DateOnly Date { get; init; }
    public required string Description { get; init; }
    public required string Period { get; init; }
    public required List<JournalEntryDetailResponse> JournalDetails { get; init; } = [];
}
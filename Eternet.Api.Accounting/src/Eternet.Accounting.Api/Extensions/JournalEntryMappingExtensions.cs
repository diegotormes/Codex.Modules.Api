namespace Eternet.Accounting.Api.Extensions;

public static class JournalEntryMappingExtensions
{
    public static JournalEntryDetailResponse ToDetailResponse(this JournalEntryDetail d)
    {
        return new JournalEntryDetailResponse
        {
            GeneralLedgerAccountId = d.GeneralLedgerAccountId,
            Debit = d.Debit,
            Credit = d.Credit,
        };
    }
}
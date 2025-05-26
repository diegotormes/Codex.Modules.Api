using System.Linq.Expressions;

namespace Eternet.Accounting.Api.Extensions;

public class JournalEntryMappingExpressions
{
    public static Expression<Func<JournalEntryDetail, JournalEntryDetailResponse>> ToDetailResponseExpression()
        => static d => d.ToDetailResponse();

    public static Expression<Func<JournalEntry, JournalEntryResponse>> ToJournalEntryResponseExpression()
    {
        var detailExpression = ToDetailResponseExpression();
#pragma warning disable CA1305 // Specify IFormatProvider
        return journalEntry => new JournalEntryResponse
        {
            Id = journalEntry.Guid,
            Date = journalEntry.Date,
            Description = journalEntry.Description,
            Period = journalEntry.AccountingPeriodId == null ? nameof(JournalEntryPeriodStatus.Pending) :
                     journalEntry.AccountingPeriodId == -2 ? nameof(JournalEntryPeriodStatus.Current) :
                     journalEntry.AccountingPeriodId.Value.ToString() ?? "",
            JournalDetails = journalEntry.AccountingEntryDetails.AsQueryable().Select(detailExpression).ToList(),
        };
#pragma warning restore CA1305 // Specify IFormatProvider
    }
}

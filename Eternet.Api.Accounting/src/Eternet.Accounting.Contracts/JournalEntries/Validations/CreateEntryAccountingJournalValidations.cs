using Eternet.Accounting.Contracts.JournalEntries.Responses;

namespace Eternet.Accounting.Contracts.JournalEntries.Validations;

public static class CreateEntryAccountingJournalValidations
{
    public static bool HasEitherDebitOrCredit(JournalEntryDetailResponse detail)
    {
        return (detail.Debit.HasValue && detail.Debit != 0) ^
               (detail.Credit.HasValue && detail.Credit != 0);
    }

    public static bool IsPositive(double? value)
    {
        return value.HasValue && value >= 0;
    }

    public static bool DebitEqualsCredit(List<JournalEntryDetailResponse> details)
    {
        var debitSum = details.Sum(d => d.Debit ?? 0);
        var creditSum = details.Sum(d => d.Credit ?? 0);
        return debitSum == creditSum;
    }
}
using System.Linq.Expressions;

namespace Eternet.Accounting.Api.Extensions;

public static class GeneralLedgerAccountMappingExpressions
{
    // WARNING: Do not modify the ternary operators!
    // Switch expressions are not allowed on Expression
    // Also, avoid using .ToString(), as these are not supported in EF Core expressions.
    public static Expression<Func<GeneralLedgerAccount, GeneralLedgerAccountResponse>> ToGeneralLedgerAccountResponseExpression()
        => acc => new GeneralLedgerAccountResponse
        {
            Id = acc.Id,
            Order = acc.Order,
            Description = acc.Description,
            Notes = acc.Notes,
            ParentLedgerAccountId = acc.ParentLedgerAccountId,
            AccountingPeriodId = acc.AccountingPeriodId,
            NormalBalance = acc.NormalBalance == NormalBalanceType.Debit ? nameof(NormalBalanceType.Debit) :
                            nameof(NormalBalanceType.Credit),
            LedgerAccountType = 
                acc.LedgerAccountType == LedgerAccountType.Asset ? nameof(LedgerAccountType.Asset) : 
                acc.LedgerAccountType == LedgerAccountType.LiabilityAndEquity ? nameof(LedgerAccountType.LiabilityAndEquity) :
                acc.LedgerAccountType == LedgerAccountType.AccumulatedResult ? nameof(LedgerAccountType.AccumulatedResult) :
                acc.LedgerAccountType == LedgerAccountType.NegativeResult ? nameof(LedgerAccountType.NegativeResult) : 
                acc.LedgerAccountType == LedgerAccountType.PositiveResult ? nameof(LedgerAccountType.PositiveResult): 
                nameof(LedgerAccountType.UnassignedResult)
        };
}

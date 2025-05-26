using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Eternet.Accounting.Api.Converters;

public static class CustomerConverters
{
    public static ValueConverter<NormalBalanceType, string> NormalBalanceTypeToString { get; } = new(
        v => v == NormalBalanceType.Debit ? "D" : "A",
        v => v == "D" ? NormalBalanceType.Debit : NormalBalanceType.Credit
    );

    public static ValueConverter<LedgerAccountType, string> LedgerAccountTypeToString { get; } = new(
            v => ConvertLedgerAccountTypeToString(v),
            v => ConvertStringToLedgerAccountType(v)
     );

    private static string ConvertLedgerAccountTypeToString(LedgerAccountType v)
    {
        return v switch
        {
            LedgerAccountType.Asset => "PA",
            LedgerAccountType.LiabilityAndEquity => "PP",
            LedgerAccountType.AccumulatedResult => "RA",
            LedgerAccountType.NegativeResult => "RN",
            LedgerAccountType.PositiveResult => "RP",
            LedgerAccountType.UnassignedResult => "RS",
            _ => throw new ArgumentOutOfRangeException(nameof(v), v, null)
        };
    }

    private static LedgerAccountType ConvertStringToLedgerAccountType(string v)
    {
        return v switch
        {
            "PA" => LedgerAccountType.Asset,
            "PP" => LedgerAccountType.LiabilityAndEquity,
            "RA" => LedgerAccountType.AccumulatedResult,
            "RN" => LedgerAccountType.NegativeResult,
            "RP" => LedgerAccountType.PositiveResult,
            "RS" => LedgerAccountType.UnassignedResult,
            _ => throw new ArgumentOutOfRangeException(nameof(v), v, null)
        };
    }
}

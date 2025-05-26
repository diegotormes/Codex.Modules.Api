using Eternet.Accounting.Api.Converters;
using Eternet.Accounting.Api.Model;

namespace Eternet.Accounting.Api.Tests.Converters;

public class CustomerConvertersTests
{
    [Theory]
    [InlineData(NormalBalanceType.Debit, "D")]
    [InlineData(NormalBalanceType.Credit, "A")]
    public void NormalBalanceType_Converter_Works(NormalBalanceType type, string expected)
    {
        var conv = CustomerConverters.NormalBalanceTypeToString;
        var toString = (string)conv.ConvertToProvider(type)!;
        toString.Should().Be(expected);
        var back = (NormalBalanceType)conv.ConvertFromProvider(toString)!;
        back.Should().Be(type);
    }

    [Theory]
    [InlineData(LedgerAccountType.Asset, "PA")]
    [InlineData(LedgerAccountType.LiabilityAndEquity, "PP")]
    [InlineData(LedgerAccountType.AccumulatedResult, "RA")]
    [InlineData(LedgerAccountType.NegativeResult, "RN")]
    [InlineData(LedgerAccountType.PositiveResult, "RP")]
    [InlineData(LedgerAccountType.UnassignedResult, "RS")]
    public void LedgerAccountType_Converter_Works(LedgerAccountType type, string expected)
    {
        var conv = CustomerConverters.LedgerAccountTypeToString;
        var toString = (string)conv.ConvertToProvider(type)!;
        toString.Should().Be(expected);
        var back = (LedgerAccountType)conv.ConvertFromProvider(toString)!;
        back.Should().Be(type);
    }
}

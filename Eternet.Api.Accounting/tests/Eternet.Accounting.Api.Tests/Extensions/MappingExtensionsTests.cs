using Eternet.Accounting.Api.Extensions;
using Eternet.Accounting.Api.Model;
using Eternet.Accounting.Contracts.JournalEntries.Responses;
using Eternet.Accounting.Api.Features.VatClosures.Preview;

namespace Eternet.Accounting.Api.Tests.Extensions;

public class MappingExtensionsTests
{
    [Fact]
    public void JournalEntryDetail_ToResponse_MapsFields()
    {
        var detail = new JournalEntryDetail { GeneralLedgerAccountId = 1, Debit = 10, Credit = 0 };
        var response = detail.ToDetailResponse();

        response.GeneralLedgerAccountId.Should().Be(1);
        response.Debit.Should().Be(10);
        response.Credit.Should().Be(0);
    }

    [Fact]
    public void JournalEntryMappingExpression_MapsPeriodText()
    {
        var entry = new JournalEntry
        {
            Guid = Guid.NewGuid(),
            Date = new DateOnly(2024,5,1),
            Description = "d",
            AccountingPeriodId = -2,
            AccountingEntryDetails = [new JournalEntryDetail { GeneralLedgerAccountId = 1, Debit = 1 }]
        };

        var func = JournalEntryMappingExpressions.ToJournalEntryResponseExpression().Compile();
        var response = func(entry);

        response.Period.Should().Be(nameof(JournalEntryPeriodStatus.Current));
    }

    [Fact]
    public void JournalEntryMappingExpression_PendingPeriod_ReturnsPending()
    {
        var entry = new JournalEntry
        {
            Guid = Guid.NewGuid(),
            Date = new DateOnly(2024, 5, 1),
            Description = "d",
            AccountingPeriodId = null,
            AccountingEntryDetails = [new JournalEntryDetail { GeneralLedgerAccountId = 1, Debit = 1 }]
        };

        var func = JournalEntryMappingExpressions.ToJournalEntryResponseExpression().Compile();
        var response = func(entry);

        response.Period.Should().Be(nameof(JournalEntryPeriodStatus.Pending));
    }

    [Fact]
    public void JournalEntryMappingExpression_ClosedPeriod_ReturnsPeriodId()
    {
        var entry = new JournalEntry
        {
            Guid = Guid.NewGuid(),
            Date = new DateOnly(2024, 5, 1),
            Description = "d",
            AccountingPeriodId = 3,
            AccountingEntryDetails = [new JournalEntryDetail { GeneralLedgerAccountId = 1, Debit = 1 }]
        };

        var func = JournalEntryMappingExpressions.ToJournalEntryResponseExpression().Compile();
        var response = func(entry);

        response.Period.Should().Be("3");
    }

    [Fact]
    public void GeneralLedgerAccountMappingExpression_MapsFields()
    {
        var account = new GeneralLedgerAccount
        {
            Id = 1,
            Order = 3,
            Description = "acc",
            Notes = "n",
            ParentLedgerAccountId = 2,
            AccountingPeriodId = 5,
            NormalBalance = NormalBalanceType.Credit,
            LedgerAccountType = LedgerAccountType.LiabilityAndEquity
        };

        var func = GeneralLedgerAccountMappingExpressions.ToGeneralLedgerAccountResponseExpression().Compile();
        var response = func(account);

        response.Id.Should().Be(account.Id);
        response.Order.Should().Be(account.Order);
        response.Description.Should().Be(account.Description);
        response.Notes.Should().Be(account.Notes);
        response.ParentLedgerAccountId.Should().Be(account.ParentLedgerAccountId);
        response.AccountingPeriodId.Should().Be(account.AccountingPeriodId);
        response.NormalBalance.Should().Be(nameof(NormalBalanceType.Credit));
        response.LedgerAccountType.Should().Be(nameof(LedgerAccountType.LiabilityAndEquity));
    }

    [Fact]
    public void VatDebitMappingExpression_MapsFields()
    {
        var entry = new VatDebitEntry
        {
            Id = 7,
            Description = "d",
            TaxResponsibility = "tr",
            VatRate = 21,
            TaxableAmount = 100,
            VatAmount = 21,
            Total = 121
        };

        var func = VatClosureMappingExpressions.ToVatDebitResponseExpression().Compile();
        var response = func(entry);

        response.Id.Should().Be(entry.Id);
        response.Description.Should().Be(entry.Description);
        response.TaxResponsibility.Should().Be(entry.TaxResponsibility);
        response.VatRate.Should().Be(entry.VatRate);
        response.TaxableAmount.Should().Be(entry.TaxableAmount);
        response.VatAmount.Should().Be(entry.VatAmount);
        response.Total.Should().Be(entry.Total);
    }

    [Fact]
    public void VatCreditMappingExpression_MapsFields()
    {
        var entry = new VatCreditEntry
        {
            Id = 9,
            Description = "c",
            VatRate = 10,
            TaxableAmount = 50,
            VatAmount = 5,
            Total = 55
        };

        var func = VatClosureMappingExpressions.ToVatCreditResponseExpression().Compile();
        var response = func(entry);

        response.Id.Should().Be(entry.Id);
        response.Description.Should().Be(entry.Description);
        response.VatRate.Should().Be(entry.VatRate);
        response.TaxableAmount.Should().Be(entry.TaxableAmount);
        response.VatAmount.Should().Be(entry.VatAmount);
        response.Total.Should().Be(entry.Total);
    }

    [Fact]
    public void VatRetentionMappingExpression_MapsFields()
    {
        var entry = new VatRetentionEntry
        {
            Id = 11,
            Date = new DateOnly(2024,6,2),
            RetentionType = "type",
            Amount = 2,
            Name = "name",
            TaxNumber = "tx",
            RetentionCode = "rc",
            CertificateNumber = "cn"
        };

        var func = VatClosureMappingExpressions.ToVatRetentionResponseExpression().Compile();
        var response = func(entry);

        response.Id.Should().Be(entry.Id);
        response.Date.Should().Be(entry.Date);
        response.RetentionType.Should().Be(entry.RetentionType);
        response.Amount.Should().Be(entry.Amount);
        response.Name.Should().Be(entry.Name);
        response.TaxNumber.Should().Be(entry.TaxNumber);
        response.RetentionCode.Should().Be(entry.RetentionCode);
        response.CertificateNumber.Should().Be(entry.CertificateNumber);
    }

    [Fact]
    public void VatSalaryMappingExpression_MapsFields()
    {
        var entry = new VatSalaryEntry
        {
            Month = 5,
            Year = 2024,
            Id = 3,
            Description = "s",
            Total = 200,
            IvaCf = 50
        };

        var func = VatClosureMappingExpressions.ToVatSalaryResponseExpression().Compile();
        var response = func(entry);

        response.Id.Should().Be(entry.Id);
        response.Month.Should().Be(entry.Month);
        response.Year.Should().Be(entry.Year);
        response.Description.Should().Be(entry.Description);
        response.Total.Should().Be(entry.Total);
        response.IvaCf.Should().Be(entry.IvaCf);
    }
}

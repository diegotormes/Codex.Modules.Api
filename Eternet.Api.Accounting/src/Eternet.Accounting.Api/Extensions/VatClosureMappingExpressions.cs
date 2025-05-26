using System.Linq.Expressions;

namespace Eternet.Accounting.Api.Extensions;

public static class VatClosureMappingExpressions
{
    public static Expression<Func<VatDebitEntry, VatDebitResponse>> ToVatDebitResponseExpression()
        => d => new VatDebitResponse
        {
            Id = d.Id,
            Description = d.Description,
            TaxResponsibility = d.TaxResponsibility,
            VatRate = d.VatRate,
            TaxableAmount = d.TaxableAmount,
            VatAmount = d.VatAmount,
            Total = d.Total
        };

    public static Expression<Func<VatCreditEntry, VatCreditResponse>> ToVatCreditResponseExpression()
        => c => new VatCreditResponse
        {
            Id = c.Id,
            Description = c.Description,
            VatRate = c.VatRate,
            TaxableAmount = c.TaxableAmount,
            VatAmount = c.VatAmount,
            Total = c.Total
        };

    public static Expression<Func<VatRetentionEntry, VatRetentionResponse>> ToVatRetentionResponseExpression()
        => r => new VatRetentionResponse
        {
            Id = r.Id,
            Date = r.Date,
            RetentionType = r.RetentionType,
            Amount = r.Amount,
            Name = r.Name,
            TaxNumber = r.TaxNumber,
            RetentionCode = r.RetentionCode,
            CertificateNumber = r.CertificateNumber
        };

    public static Expression<Func<VatSalaryEntry, VatSalaryResponse>> ToVatSalaryResponseExpression()
        => s => new VatSalaryResponse
        {
            Id = s.Id,
            Month = s.Month,
            Year = s.Year,
            Description = s.Description,
            Total = s.Total,
            IvaCf = s.IvaCf
        };
}

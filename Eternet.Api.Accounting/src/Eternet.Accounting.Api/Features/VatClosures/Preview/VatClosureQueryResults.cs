namespace Eternet.Accounting.Api.Features.VatClosures.Preview;

public class VatDebitEntry
{
    public int IdInternal { get; set; }
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string TaxResponsibility { get; set; } = string.Empty;
    public decimal TaxableAmount { get; set; }
    public decimal VatRate { get; set; }
    public decimal VatAmount { get; set; }
    public decimal Total { get; set; }
}

public class VatCreditEntry
{
    public int IdInternal { get; set; }
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal TaxableAmount { get; set; }
    public decimal VatRate { get; set; }
    public decimal VatAmount { get; set; }
    public decimal Total { get; set; }
}

public class VatRetentionEntry
{
    public int IdInternal { get; set; }
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string RetentionType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
    public string RetentionCode { get; set; } = string.Empty;
    public string? CertificateNumber { get; set; }
}

public class VatSalaryEntry
{
    public int Id { get; set; }
    public short Month { get; set; }
    public short Year { get; set; }    
    public string Description { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public decimal IvaCf { get; set; }
}

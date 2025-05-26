namespace Eternet.Purchasing.Contracts.Invoices;

public record InvoiceResponse
{
    public Guid Id { get; init; }
    public int InternalId { get; init; }
    public string InvoiceType { get; init; } = string.Empty;
    public string VoucherType { get; init; } = string.Empty;
    public string BranchNumber { get; init; } = string.Empty;
    public string VoucherNumber { get; init; } = string.Empty;
    public int? VendorId { get; init; }
    public string VendorTaxId { get; init; } = string.Empty;
    public string VendorName { get; init; } = string.Empty;
    public DateTime IssueDate { get; init; }
    public string Description { get; init; } = string.Empty;
    public decimal? TotalAmount { get; init; }
    public decimal? Subtotal { get; init; }
    public decimal? VatTotalAmount { get; init; }
    public decimal? NetAmount { get; init; }
    public decimal? ExemptAmount { get; init; }
    public decimal? NonTaxableAmount { get; init; }
    public decimal? VatRetentionTotalAmount { get; init; }
    public decimal? VatPerceptionTotalAmount { get; init; }
    public decimal? IbRetentionTotalAmount { get; init; }
    public decimal? IbPerceptionTotalAmount { get; init; }
    public decimal? ProfitRetentionTotalAmount { get; init; }
    public decimal? ProfitPerceptionTotalAmount { get; init; }
    public DateTime? DueDate { get; init; }
    public List<InvoiceDetailResponse> Details { get; init; } = [];
    public List<InvoiceRetentionResponse> Retentions { get; init; } = [];
    public List<InvoicePaymentMethodResponse> PaymentMethods { get; init; } = [];
    public List<VendorCurrentAccountResponse> VendorCurrentAccounts { get; init; } = [];
}

public record InvoiceDetailResponse
{
    public int Id { get; init; }
    public required string ServiceArticleId { get; init; }
    public required string Description { get; init; }
    public float Quantity { get; init; }
    public decimal VatRate { get; init; }
    public decimal? Taxable { get; init; }
    public decimal? Subtotal { get; init; }
    public decimal? Vat { get; init; }
    public decimal? NonTaxable { get; init; }
    public decimal? Exempt { get; init; }
    public decimal? TotalDetail { get; init; }
}

public record InvoiceRetentionResponse
{
    public int Id { get; init; }
    public int RetentionType { get; init; }
    public required string RetentionTypeDescription { get; init; }
    public int? ProvinceId { get; init; }
    public string? ProvinceDescription { get; init; }
    public decimal TotalAmount { get; init; }
}

public record InvoicePaymentMethodResponse
{
    public int Id { get; init; }
    public required string Description { get; init; }
    public DateTime? Date { get; init; }
    public decimal TotalAmount { get; init; }
    public int PaymentMethodId { get; init; }
    public required string PaymentMethodDescription { get; init; }
}

public record VendorCurrentAccountResponse
{
    public int Id { get; init; }
    public int Provider { get; init; }
    public DateTime Date { get; init; }
    public string InvoiceType { get; init; } = string.Empty;
    public string VoucherType { get; init; } = string.Empty;
    public string BranchNumber { get; init; } = string.Empty;
    public string VoucherNumber { get; init; } = string.Empty;
    public float? Debit { get; init; }
    public decimal? Credit { get; init; }
    public float? Balance { get; init; }
    public string Observations { get; init; } = string.Empty;
    public int? InvoiceId { get; init; }
    public int? PointOfSale { get; init; }
    public float AdvancePayment { get; init; }
    public string User { get; init; } = string.Empty;
    public short Security { get; init; }
}
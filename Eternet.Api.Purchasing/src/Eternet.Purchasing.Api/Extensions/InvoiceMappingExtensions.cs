using Eternet.Purchasing.Contracts.Invoices;

namespace Eternet.Purchasing.Api.Extensions;

public static class InvoiceMappingExtensions
{
    public static VendorCurrentAccount CreateVendorCurrentAccount(this Invoice invoice)
    {
        var vendorAccount = new VendorCurrentAccount
        {
            Provider = invoice.VendorId ?? 0,
            Date = invoice.IssueDate,
            InvoiceType = invoice.InvoiceType,
            VoucherType = invoice.VoucherType,
            BranchNumber = invoice.BranchNumber,
            VoucherNumber = invoice.VoucherNumber,
            Debit = null,
            Credit = invoice.TotalAmount,
            Observations = invoice.Description,
            InvoiceId = invoice.Id,
            PointOfSale = invoice.SalesPoint,
        };
        return vendorAccount;
    }
    public static InvoiceDetailResponse ToDetailResponse(this InvoiceDetail detail)
    {
        return new InvoiceDetailResponse
        {
            Id = detail.Id,
            ServiceArticleId = detail.ServiceArticleId,
            Description = detail.Description,
            Quantity = detail.Quantity,
            VatRate = detail.VatRate,
            Taxable = detail.Taxable,
            Subtotal = detail.Subtotal,
            Vat = detail.Vat,
            NonTaxable = detail.NonTaxable,
            Exempt = detail.Exempt,
            TotalDetail = detail.TotalDetail
        };
    }

    public static InvoiceRetentionResponse ToRetentionResponse(this InvoicePurchaseRetention retention)
    {
        return new InvoiceRetentionResponse
        {
            Id = retention.Id,
            RetentionType = retention.RetentionType,
            RetentionTypeDescription = ((RetentionTypes)retention.RetentionType).ToString(),
            ProvinceId = retention.ProvinceId,
            ProvinceDescription = retention.Province?.Description,
            TotalAmount = retention.TotalAmount
        };
    }

    public static InvoicePaymentMethodResponse ToPaymentMethodResponse(this PurchasePaymentMethod paymentMethod)
    {
        return new InvoicePaymentMethodResponse
        {
            Id = paymentMethod.Id,
            Description = paymentMethod.Description,
            Date = paymentMethod.Date,
            TotalAmount = paymentMethod.TotalAmount,
            PaymentMethodId = paymentMethod.PaymentMethodId,
            PaymentMethodDescription = paymentMethod.PaymentMethod?.Description ?? string.Empty
        };
    }
}
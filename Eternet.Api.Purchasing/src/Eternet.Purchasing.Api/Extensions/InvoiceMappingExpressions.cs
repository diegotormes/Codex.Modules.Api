using System.Linq.Expressions;
using Eternet.Purchasing.Contracts.Invoices;

namespace Eternet.Purchasing.Api.Extensions;
public static class InvoiceMappingExpressions
{
    public static Expression<Func<InvoiceDetail, InvoiceDetailResponse>> ToDetailResponseExpression()
        => static d => d.ToDetailResponse();

    public static Expression<Func<InvoicePurchaseRetention, InvoiceRetentionResponse>> ToRetentionResponseExpression()
        => static r => r.ToRetentionResponse();

    public static Expression<Func<PurchasePaymentMethod, InvoicePaymentMethodResponse>> ToPaymentMethodResponseExpression()
        => static pm => pm.ToPaymentMethodResponse();

    public static Expression<Func<VendorCurrentAccount, VendorCurrentAccountResponse>> ToVendorCurrentAccountResponseExpression()
        => vca => new VendorCurrentAccountResponse
        {
            Id = vca.Id,
            Provider = vca.Provider,
            Date = vca.Date,
            InvoiceType = vca.InvoiceType,
            VoucherType = vca.VoucherType,
            BranchNumber = vca.BranchNumber,
            VoucherNumber = vca.VoucherNumber,
            Debit = vca.Debit,
            Credit = vca.Credit,
            Balance = vca.Balance,
            Observations = vca.Observations,
            InvoiceId = vca.InvoiceId,
            PointOfSale = vca.PointOfSale,
            AdvancePayment = vca.AdvancePayment,
            User = vca.User,
            Security = vca.Security
        };

    public static Expression<Func<Invoice, InvoiceResponse>> ToInvoiceResponseExpression()
    {
        var detailExpression = ToDetailResponseExpression();
        var retentionExpression = ToRetentionResponseExpression();
        var paymentMethodExpression = ToPaymentMethodResponseExpression();
        var vendorCurrentAccountExpression = ToVendorCurrentAccountResponseExpression();
        return invoice => new InvoiceResponse
        {
            Id = invoice.Guid,
            InternalId = invoice.Id,
            InvoiceType = invoice.InvoiceType,
            VoucherType = invoice.VoucherType,
            BranchNumber = invoice.BranchNumber,
            VoucherNumber = invoice.VoucherNumber,
            VendorId = invoice.VendorId,
            VendorTaxId = invoice.VendorTaxId,
            VendorName = invoice.VendorName,
            IssueDate = invoice.IssueDate,
            Description = invoice.Description,
            TotalAmount = invoice.TotalAmount,
            Subtotal = invoice.Subtotal,
            VatTotalAmount = invoice.VatTotalAmount,
            NetAmount = invoice.NetAmount,
            ExemptAmount = invoice.ExemptAmount,
            NonTaxableAmount = invoice.NonTaxableAmount,
            VatRetentionTotalAmount = invoice.VatRetentionTotalAmount,
            VatPerceptionTotalAmount = invoice.VatPerceptionTotalAmount,
            IbRetentionTotalAmount = invoice.IbRetentionTotalAmount,
            IbPerceptionTotalAmount = invoice.IbPerceptionTotalAmount,
            ProfitRetentionTotalAmount = invoice.ProfitRetentionTotalAmount,
            ProfitPerceptionTotalAmount = invoice.ProfitPerceptionTotalAmount,
            DueDate = invoice.DueDate,
            Details = invoice.Details.AsQueryable().Select(detailExpression).ToList(),
            Retentions = invoice.PurchaseRetentions.AsQueryable().Select(retentionExpression).ToList(),
            PaymentMethods = invoice.PurchasePaymentMethods.AsQueryable().Select(paymentMethodExpression).ToList(),
            VendorCurrentAccounts = invoice.VendorCurrentAccounts.AsQueryable().Select(vendorCurrentAccountExpression).ToList()
        };
    }
}

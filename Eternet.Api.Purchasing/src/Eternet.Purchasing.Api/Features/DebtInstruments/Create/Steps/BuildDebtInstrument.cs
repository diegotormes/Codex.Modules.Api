using Eternet.Purchasing.Api.Extensions;
using Eternet.Purchasing.Contracts.Liabilities;

namespace Eternet.Purchasing.Api.Features.DebtInstruments.Create.Steps;

public class BuildDebtInstrument
{
    public Invoice Handle(CreateDebtInstrument.Request request, VendorValidated vendor)
    {
        var newDebtInstrument = new Invoice
        {
            Guid = Guid.CreateVersion7(),
            InvoiceType = "C",
            VoucherType = "F",
            BranchNumber = "0000",
            VoucherNumber = "00000000",
            VendorId = vendor.Id,
            VendorName = vendor.Name,
            VendorTaxId = request.VendorTaxId,
            SalesPoint = 99,
            VatStatus = vendor.VatStatus,
            IssueDate = request.IssueDate.ToDateTime(TimeOnly.MinValue),
            Description = request.Description,
            VatTotalAmount = 0,
            TotalAmount = request.TotalAmount,
            PaymentMethodId = vendor.PaymentMethodId,
            DueDate = request.DueDate.ToDateTime(TimeOnly.MinValue),
            Details = 
            [
                new InvoiceDetail
                {
                    ServiceArticleId = request.ExpenseId.ToString(),
                    Description = request.Description,
                    Quantity = 1,
                    NonTaxable = request.TotalAmount,
                    TotalDetail = request.TotalAmount
                }
            ]
        };
        newDebtInstrument.VendorCurrentAccounts.Add(newDebtInstrument.CreateVendorCurrentAccount());
        return newDebtInstrument;
    }
}
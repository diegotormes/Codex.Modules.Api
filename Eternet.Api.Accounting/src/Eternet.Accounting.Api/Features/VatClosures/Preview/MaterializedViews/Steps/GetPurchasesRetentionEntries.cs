namespace Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;

public class GetPurchasesRetentionEntries(AccountingContext dbContext) : BaseHandlerResponses
{
    private const string PurchasesQuery = "PurchasesQuery";

    [KeyedResult(PurchasesQuery)]
    public IQueryable<VatRetentionEntry> Handle(
        RefreshVatRetentionViewHandler.Request request)
    {
        return dbContext.PurchaseRetentions
            .Where(r =>
                r.Month == request.Month &&
                r.Year == request.Year &&
                (r.RetentionTypeId == 1 || r.RetentionTypeId == 6))
            .Join(dbContext.RetentionTypes,
                r => r.RetentionTypeId,
                t => t.Id,
                (r, t) => new { r, t })
            .Join(dbContext.VatPurchases,
                rt => rt.r.VatPurchaseId,
                i => i.Id,
                (rt, i) => new { rt.r, rt.t, i })
            .GroupJoin(dbContext.PurchaseInvoices,
                rti => rti.i.InvoiceId,
                fc => fc.Id,
                (rti, fcJoin) => new { rti, fcJoin })
            .SelectMany(
                x => x.fcJoin.DefaultIfEmpty(),
                (x, fc) => new { x.rti.r, x.rti.t, x.rti.i, fc })
            .GroupJoin(dbContext.PurchaseRetentionDetails,
                rtti => rtti.r.PurchaseRetentionDetailId,
                fr => fr.Id,
                (rtti, frJoin) => new { rtti, frJoin })
            .SelectMany(
                x => x.frJoin.DefaultIfEmpty(),
                (x, fr) => new VatRetentionEntry
                {
                    Id = x.rtti.r.Id,
                    Date = x.rtti.i.InvoiceDate,
                    RetentionType = x.rtti.t.Description,
                    Amount = x.rtti.r.RetentionTypeId == 1
                        ? (fr != null
                            ? (x.rtti.fc != null && x.rtti.fc.VoucherType == "NC" ? -fr.Amount : fr.Amount)
                            : x.rtti.i.VatRetention ?? 0)
                        : (fr != null
                            ? (x.rtti.fc != null && x.rtti.fc.VoucherType == "NC" ? -fr.Amount : fr.Amount)
                            : x.rtti.i.VatPerception ?? 0),
                    Name = x.rtti.i.Provider,
                    TaxNumber = x.rtti.i.Cuit,
                    RetentionCode = x.rtti.r.RetentionCode,
                    CertificateNumber = x.rtti.r.CertificateNumber
                });
    }
}

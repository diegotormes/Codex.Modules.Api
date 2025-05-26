namespace Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;

public class GetSalesRetentionEntries(AccountingContext dbContext) : BaseHandlerResponses
{
    private const string SalesQuery = "SalesQuery";

    [KeyedResult(SalesQuery)]
    public IQueryable<VatRetentionEntry> Handle(RefreshVatRetentionViewHandler.Request request)
    {
        var sales = dbContext.SalesRetentions
            .Where(s =>
                s.Month == request.Month &&
                s.Year == request.Year &&
                (s.RetentionTypeId == 1 || s.RetentionTypeId == 6))
            .Join(dbContext.RetentionTypes,
                  s => s.RetentionTypeId, t => t.Id,
                  (s, t) => new { s, t })
            .Join(dbContext.CustomerAccounts,
                  st => st.s.MovementId, cc => cc.Id,
                  (st, cc) => new { st.s, st.t, cc })
            .Join(dbContext.Customers,
                  stcc => stcc.cc.ClientId, c => c.Id,
                  (stcc, c) => new VatRetentionEntry
                  {
                      Id = stcc.s.Id,
                      Date = stcc.s.Date,
                      RetentionType = stcc.t.Description,
                      Amount = stcc.s.Amount,
                      Name = c.Name,
                      TaxNumber = c.TaxNumber,
                      RetentionCode = stcc.s.RetentionCode,
                      CertificateNumber = stcc.s.CertificateNumber
                  });

        return sales;
    }
}

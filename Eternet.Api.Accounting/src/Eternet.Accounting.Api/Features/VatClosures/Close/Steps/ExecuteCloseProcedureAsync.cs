using Eternet.Accounting.Contracts.VatClosures;

namespace Eternet.Accounting.Api.Features.VatClosures.Close.Steps;

public class ExecuteCloseProcedureAsync(AccountingContext dbContext) : BaseHandlerResponses
{
    public async Task<StepResult> Handle(CloseVat.Request request, VatClosesData totals, CancellationToken cancellationToken)
    {
        var amount = totals.Debit - totals.Credit - totals.Retention - totals.SocialSecurity;
        await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXECUTE PROCEDURE IVA_CIERRE_AGREGAR {request.Month}, {request.Year}, {amount}, {request.DueDate}", cancellationToken);
        return Next(cancellationToken);
    }
}

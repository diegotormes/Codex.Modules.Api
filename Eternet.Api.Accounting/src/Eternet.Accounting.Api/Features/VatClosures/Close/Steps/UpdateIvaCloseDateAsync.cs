using Eternet.Accounting.Contracts.VatClosures;

namespace Eternet.Accounting.Api.Features.VatClosures.Close.Steps;

public class UpdateIvaCloseDateAsync(AccountingContext dbContext) : BaseHandlerResponses
{
    public async Task<StepResult> Handle(CloseVat.Request request, CancellationToken cancellationToken)
    {
        await dbContext.Database.ExecuteSqlInterpolatedAsync($"update IVA_CIERRE set FECHA_CIERRE = {request.CloseDate}", cancellationToken);
        return Next(cancellationToken);
    }
}

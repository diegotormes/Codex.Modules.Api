namespace Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;

public class ClearVatCreditViewAsync(ViewsContext viewContext) : BaseHandlerResponses
{
    public async Task<StepResult> Handle(RefreshVatCreditViewHandler.Request request, CancellationToken cancellationToken)
    {
        viewContext.VatCredits.RemoveRange(viewContext.VatCredits.Where(x => x.VatRate >= 0));
        await viewContext.SaveChangesAsync(cancellationToken);
        return Next(cancellationToken);
    }
}

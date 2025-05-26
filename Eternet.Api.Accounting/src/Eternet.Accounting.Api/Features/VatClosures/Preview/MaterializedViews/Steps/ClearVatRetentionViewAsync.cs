namespace Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;

public class ClearVatRetentionViewAsync(ViewsContext viewContext) : BaseHandlerResponses
{
    public async Task<StepResult> Handle(RefreshVatRetentionViewHandler.Request request, CancellationToken cancellationToken)
    {
        viewContext.VatRetentions.RemoveRange(viewContext.VatRetentions.Where(x => x.Date.Year == request.Year && x.Date.Month == request.Month));
        await viewContext.SaveChangesAsync(cancellationToken);
        return Next(cancellationToken);
    }
}

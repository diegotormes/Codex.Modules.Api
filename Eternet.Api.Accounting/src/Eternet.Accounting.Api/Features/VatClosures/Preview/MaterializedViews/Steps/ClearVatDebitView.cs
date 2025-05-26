namespace Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;

public class ClearVatDebitViewAsync(ViewsContext viewContext) : BaseHandlerResponses
{
    public async Task<StepResult> Handle(RefreshVatDebitViewHandler.Request request, CancellationToken cancellationToken)
    {
        viewContext.VatDebits.RemoveRange(viewContext.VatDebits.Where(x => x.VatRate >= 0));
        await viewContext.SaveChangesAsync(cancellationToken);
        return Next(cancellationToken);
    }
}

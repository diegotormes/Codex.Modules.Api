namespace Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;

public class SaveVatCreditEntriesAsync(ViewsContext viewContext) : BaseHandlerResponses
{
    const int BatchSize = 50;

    public async Task<StepResult> Handle(IQueryable<VatCreditEntry> entries, CancellationToken cancellationToken)
    {
        var batch = new List<VatCreditEntry>(BatchSize);
        await foreach (var entry in entries.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            batch.Add(entry);
            if (batch.Count >= BatchSize)
            {
                await viewContext.VatCredits.AddRangeAsync(batch, cancellationToken);
                await viewContext.SaveChangesAsync(cancellationToken);
                batch.Clear();
            }
        }

        if (batch.Count > 0)
        {
            await viewContext.VatCredits.AddRangeAsync(batch, cancellationToken);
            await viewContext.SaveChangesAsync(cancellationToken);
        }

        return Next(cancellationToken);
    }
}

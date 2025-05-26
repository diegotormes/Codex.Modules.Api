namespace Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews;

using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;

[GenerateExecutePipeline<ClearVatRetentionViewAsync>]
[GenerateExecutePipeline<GetSalesRetentionEntries>]
[GenerateExecutePipeline<GetPurchasesRetentionEntries>]
[GenerateExecutePipeline<SaveVatRetentionEntriesAsync>]
public class RefreshVatRetentionViewHandler : DomainResultHandler<RefreshVatRetentionViewHandler.Request, RefreshVatRetentionViewHandler.Response>
{
    public record Request : IRequest<Response>
    {
        public required short Month { get; init; }
        public required short Year { get; init; }
    }

    public record Response : DomainResult;

    public override Response Handle(Request request) => new();
}

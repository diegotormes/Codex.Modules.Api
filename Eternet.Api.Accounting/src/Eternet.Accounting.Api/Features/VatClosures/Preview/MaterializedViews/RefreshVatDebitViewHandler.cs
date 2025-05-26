namespace Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews;

using Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews.Steps;

[GenerateExecutePipeline<ClearVatDebitViewAsync>]
[GenerateExecutePipeline<GetVatDebitEntries>]
[GenerateExecutePipeline<SaveVatDebitEntriesAsync>]
public class RefreshVatDebitViewHandler : DomainResultHandler<RefreshVatDebitViewHandler.Request, RefreshVatDebitViewHandler.Response>
{
    public record Request : IRequest<Response>
    {
        public required short Month { get; init; }
        public required short Year { get; init; }
    }

    public record Response : DomainResult;

    public override Response Handle(Request request) => new();
}

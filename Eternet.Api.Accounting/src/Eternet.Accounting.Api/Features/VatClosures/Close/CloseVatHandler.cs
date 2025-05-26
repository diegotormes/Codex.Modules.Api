using Eternet.Accounting.Api.Features.VatClosures.Close.Steps;
using Eternet.Accounting.Contracts.VatClosures;

namespace Eternet.Accounting.Api.Features.VatClosures.Close;

[HandlerForEntity<VatSalaryClosureDetail>]
[GenerateExecutePipeline<LoadSalariesAsync>]
[GenerateExecutePipeline<LoadVatDataAsync>]
[GenerateExecutePipeline<SaveSalaryDetailsAsync>]
[GenerateExecutePipeline<ExecuteCloseProcedureAsync>]
[GenerateExecutePipeline<UpdateIvaCloseDateAsync>]
public class CloseVatHandler : CloseVat
{
    public override Response Handle(Request request)
    {
        return new Response { CloseDate = request.CloseDate };
    }
}

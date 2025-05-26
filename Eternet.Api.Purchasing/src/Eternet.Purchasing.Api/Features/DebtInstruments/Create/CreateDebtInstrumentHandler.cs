using Eternet.Purchasing.Api.Features.DebtInstruments.Create.Steps;
using Eternet.Purchasing.Contracts.Liabilities;

namespace Eternet.Purchasing.Api.Features.Create;

[HandlerForEntity<Invoice>]
[GenerateExecutePipeline<ValidateVendorExistsAsync>]
[GenerateExecutePipeline<ValidateExpenseExistsAsync>]
[GenerateExecutePipeline<BuildDebtInstrument>]
public partial class CreateDebtInstrumentHandler(ScopedStates scopedStates) : CreateDebtInstrument
{
    public override Response Handle(Request request)
    {
        var debtInstrument = scopedStates.GetRequiredState<Invoice>();

        var response = new Response
        {
            Id = debtInstrument.Guid,
            Entity = debtInstrument
        };

        return response;
    }
}
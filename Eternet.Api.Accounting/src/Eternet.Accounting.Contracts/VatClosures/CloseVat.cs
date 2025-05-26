using Eternet.Mediator.Attributes.Mvc;

namespace Eternet.Accounting.Contracts.VatClosures;

[GenerateEndpoint(Config.ControllerName)]
public abstract class CloseVat : DomainResultHandler<CloseVat.Request, CloseVat.Response>
{
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Month).InclusiveBetween((short)1, (short)12);
            RuleFor(x => x.Year).GreaterThan((short)2000);
            RuleFor(x => x.CloseDate).NotEmpty();
            RuleFor(x => x.DueDate).NotEmpty();
        }
    }

    [FromBody]
    public record Request : CreateEntityCommand<Response>
    {
        public required short Month { get; init; }
        public required short Year { get; init; }
        public required DateOnly CloseDate { get; init; }
        public required DateOnly DueDate { get; init; }
    }

    public record Response : CreateEntityDomainResult
    {
        public required DateOnly CloseDate { get; set; }
    }

    [HttpPost(Config.VatClosures)]
    public abstract override Response Handle(Request request);
}

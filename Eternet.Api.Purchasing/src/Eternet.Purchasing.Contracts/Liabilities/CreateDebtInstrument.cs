using Eternet.Mediator.Abstractions.Commands;
using Eternet.Mediator.Attributes.Mvc;

namespace Eternet.Purchasing.Contracts.Liabilities;

[GenerateEndpoint(Config.ControllerName)]
public abstract class CreateDebtInstrument : DomainResultHandler<
    CreateDebtInstrument.Request,
    CreateDebtInstrument.Response>
{
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.VendorTaxId)
                .NotEmpty()
                .WithMessage("VendorId is required.");

            RuleFor(x => x.IssueDate)
                .NotEmpty()
                .WithMessage("IssueDate is required.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(255)
                .WithMessage("Description must be at most 255 characters long.");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0)
                .WithMessage("TotalAmount must be greater than 0.");

            RuleFor(x => x.DueDate)
                .NotEmpty()
                .WithMessage("DueDate is required.")
                .GreaterThanOrEqualTo(x => x.IssueDate)
                .WithMessage("DueDate must be on or after IssueDate.");
        }
    }

    [FromBody]
    public record Request : CreateEntityCommand<Response>
    {
        public required string VendorTaxId { get; init; }
        public required DateOnly IssueDate { get; init; }
        public required string Description { get; init; }
        public required decimal TotalAmount { get; init; }
        public required int ExpenseId { get; init; }
        public required DateOnly DueDate { get; init; }
    }

    public record Response : CreateEntityDomainResult
    {
        public required Guid Id { get; set; }
    }

    [HttpPost(Config.Manual)]
    public abstract override Response Handle(Request request);
}
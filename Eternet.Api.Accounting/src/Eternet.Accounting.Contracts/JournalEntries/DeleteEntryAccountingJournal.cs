using Eternet.Mediator.Attributes.Mvc;

namespace Eternet.Accounting.Contracts.JournalEntries;

[GenerateEndpoint(Config.ControllerName)]
public abstract class DeleteEntryAccountingJournal : DomainResultHandler<
    DeleteEntryAccountingJournal.Request,
    DeleteEntryAccountingJournal.Response>
{
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required.");
        }
    }
        
    public record Request : DeleteEntityCommand<Response>
    {
        [FromRoute]
        public required Guid Id { get; init; }

        public override object GetId() => Id;
    }

    public record Response : DeleteEntityDomainResult
    {
        public required Guid Id { get; set; }
    }

    [HttpDelete($"{Config.JournalEntries}/{{id}}")]
    public abstract override Response Handle(Request request);
}

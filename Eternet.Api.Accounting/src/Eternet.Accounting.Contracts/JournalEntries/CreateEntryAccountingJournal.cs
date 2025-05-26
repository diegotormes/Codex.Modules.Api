using Eternet.Accounting.Contracts.JournalEntries.Responses;
using Eternet.Accounting.Contracts.JournalEntries.Validations;
using Eternet.Mediator.Attributes.Mvc;

namespace Eternet.Accounting.Contracts.JournalEntries;

/// <summary>
/// Creates a new journal entry in the accounting system.
/// </summary>
[GenerateEndpoint(Config.ControllerName)]
public abstract class CreateEntryAccountingJournal : DomainResultHandler<
    CreateEntryAccountingJournal.Request,
    CreateEntryAccountingJournal.Response>
{
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(100)
                .WithMessage("Description must be at most 100 characters long.");

            RuleFor(x => x.JournalDetails)
                .NotEmpty()
                .WithMessage("Journal details are required.");

            RuleForEach(x => x.JournalDetails).ChildRules(details =>
            {
                details.RuleFor(d => d.GeneralLedgerAccountId)
                    .NotEmpty()
                    .WithMessage("GeneralLedgerAccountId is required for each detail.");

                details.RuleFor(d => d)
                    .Must(CreateEntryAccountingJournalValidations.HasEitherDebitOrCredit)
                    .WithMessage("Each detail must have either a Debit or a Credit value, but not both or neither.");

                details.RuleFor(d => d.Debit)
                    .Must(CreateEntryAccountingJournalValidations.IsPositive)
                    .When(d => d.Debit.HasValue)
                    .WithMessage("Debit value must be positive.");

                details.RuleFor(d => d.Credit)
                    .Must(CreateEntryAccountingJournalValidations.IsPositive)
                    .When(d => d.Credit.HasValue)
                    .WithMessage("Credit value must be positive.");
            });

            RuleFor(x => x.JournalDetails)
                .Must(CreateEntryAccountingJournalValidations.DebitEqualsCredit)
                .WithMessage("The sum of Debit and Credit must be equal across all details.");
        }
    }

    [FromBody]
    public record Request : CreateEntityCommand<Response>
    {
        public required DateOnly Date { get; init; }
        public required JournalEntryPeriodStatus PeriodStatus { get; init; }
        public required string Description { get; init; }
        public required List<JournalEntryDetailResponse> JournalDetails { get; init; } = [];
    }

    public record Response : CreateEntityDomainResult
    {
        public required Guid Id { get; set; }
    }

    [HttpPost(Config.JournalEntries)]
    public abstract override Response Handle(Request request);
}
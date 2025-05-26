
namespace Eternet.Purchasing.Api.Features.DebtInstruments.Create.Steps;

public class ValidateExpenseExistsAsync(PurchasingContext dbContext) : BaseHandlerResponses
{
    public async Task<StepResult> Handle(int expenseId, CancellationToken cancellationToken)
    {
        var exists = await dbContext.ExpenseTypes
            .AsNoTracking()
            .AnyAsync(e => e.Id == expenseId, cancellationToken: cancellationToken);
        if (exists is false)
        {
            return InvalidStateError(GetExpenseNotFoundError(expenseId));
        }
        return Next(cancellationToken);
    }

    public string GetExpenseNotFoundError(int expenseId)
    {
        return $"Expense with Id {expenseId} not found";
    }
}

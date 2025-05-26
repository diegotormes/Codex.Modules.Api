using Microsoft.EntityFrameworkCore.Storage;
using Mediator;

namespace Eternet.Accounting.Api.Tests.Common;

public class TransactionalTestContext(
    AccountingContext context,
    IMediator mediator,
    IDbContextTransaction transaction,
    string dbPath,
    Action<string> releaseDb) : IAsyncDisposable, IDisposable
{
    public AccountingContext Context { get; } = context;
    public IMediator Mediator { get; } = mediator;

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 0)
        {
            await transaction.RollbackAsync();
            await Context.DisposeAsync();
            releaseDb(dbPath);
        }
        GC.SuppressFinalize(this);
    }

    private int _disposed;

    public void Dispose()
    {
        var valueTask = DisposeAsync();
        if (!valueTask.IsCompletedSuccessfully)
        {
            valueTask.AsTask().GetAwaiter().GetResult();
        }
    }
}

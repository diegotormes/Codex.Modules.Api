using System.Linq.Expressions;
using Eternet.Web.UI.Abstractions.DataGrid;
using ColumnName = string;

namespace Eternet.Web.UI.Abstractions.Services;

public interface IGridColumnService
{
    Dictionary<ColumnName, GridColumnMetadata<TGridItem>> GetColumnsFor<TGridItem>();
}

public interface IViewsService
{
    IQueryable<T> GetViewAsQuery<T>() where T : class;

    Task<bool> UpdateSourcePropertyAsync(object id, Type entityType, string propertyName, object newValue);
    Task<bool> UpdateViewPropertyAsync(object id, Type entityType, string propertyName, object newValue);   
}

public interface IQueriesService
{
    IQueryable<T> GetQuery<T>() where T : class;
}

public interface IQueryService<T> where T : class
{
    IQueryable<T> GetQuery();
    Task<T[]> ToListAsync(CancellationToken cancellationToken = default);
    Task<TResult[]> ToListAsync<TResult>(IQueryable<TResult> query, CancellationToken cancellationToken = default);
    Task<T[]> ToListAsync(IQueryable<T> query, CancellationToken cancellationToken = default);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
}

public interface IViewModelService<T> where T : class
{
    IQueryable<T> GetQuery();
}

public interface IBuildStringLikeExpression
{
    Expression Build(Expression columnBody, string value);
}
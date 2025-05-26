using System.Collections.Concurrent;
using System.Linq.Expressions;
using Microsoft.OData.UriParser;

namespace Eternet.Accounting.Api.Services;

public sealed class AllStringsSearchBinder : QueryBinder, ISearchBinder
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _stringProps = new();

    public Expression BindSearch(SearchClause clause, QueryBinderContext ctx)
    {
        var param = ctx.CurrentParameter;
        var props = _stringProps.GetOrAdd(param.Type, t =>
                      t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       .Where(p => p.PropertyType == typeof(string) && p.CanRead)
                       .ToArray());

        return Expression.Lambda(
                   clause.Expression.BuildPredicate(param, props),
                   param);
    }
}

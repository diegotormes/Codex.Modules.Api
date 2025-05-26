using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Eternet.Web.UI.Abstractions;
using Eternet.Web.UI.Abstractions.DataGrid;
using Eternet.Web.UI.Abstractions.Services;
using ColumnName = string;

namespace Invoices.Incoming.WebApi.Components.DataGrid;

public class GridColumnService : IGridColumnService
{
    private readonly ConcurrentDictionary<Type, object> _cache = new();

    public Dictionary<ColumnName, GridColumnMetadata<TGridItem>> GetColumnsFor<TGridItem>()
    {
        var type = typeof(TGridItem);
        if (_cache.TryGetValue(type, out var cachedMetadata))
        {
            return (Dictionary<ColumnName, GridColumnMetadata<TGridItem>>)cachedMetadata;
        }

        var metadata = type.GetProperties()
            .Select(prop =>
            {
                var attribute = prop.GetCustomAttribute<EternetGridColumnAttribute>();
                var filterAttribute = prop.GetCustomAttribute<FilterableAttribute>();
                if (attribute != null && attribute.AutoRender)
                {
                    var parameter = Expression.Parameter(typeof(TGridItem), "p");

                    var propertyAccess = Expression.Property(parameter, prop);
                    var lambdaOriginal = Expression.Lambda(propertyAccess, parameter);

                    var convert = Expression.Convert(propertyAccess, typeof(object));
                    var lambdaObject = Expression.Lambda<Func<TGridItem, object>>(convert, parameter);

                    return new GridColumnMetadata<TGridItem>
                    {
                        ColumnName = prop.Name,
                        Title = string.IsNullOrEmpty(attribute.Title) ? prop.Name : attribute.Title,
                        AllowsSorting = attribute.AllowsSorting,
                        AllowsSearching = attribute.AllowsSearching,
                        AllowsFiltering = attribute.AllowsFiltering || filterAttribute is not null,
                        AllowsIntegerFiltering = attribute.AllowsIntegerFiltering,
                        LambdaExpression = lambdaOriginal,
                        PropertyExpression = lambdaObject,
                        FilterableAttribute = filterAttribute
                    };
                }
                return null;
            })
            .Where(meta => meta != null)
            .ToDictionary(x => x!.ColumnName, x => x);

        _cache[type] = metadata;
        return metadata!;
    }

}
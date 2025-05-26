using System.Linq.Expressions;

namespace Eternet.Web.UI.Abstractions.DataGrid;

public record GridColumnMetadata<TGridItem>
{
    public required string ColumnName { get; init; }
    public required string Title { get; init; }
    public required bool AllowsSorting { get; init; }
    public required bool AllowsSearching { get; init; }
    public required bool AllowsFiltering { get; init; }
    public required bool AllowsIntegerFiltering { get; init; }
    public required LambdaExpression LambdaExpression { get; init; }
    public required Expression<Func<TGridItem, object>> PropertyExpression { get; init; }
    public required FilterableAttribute? FilterableAttribute { get; init; }
}
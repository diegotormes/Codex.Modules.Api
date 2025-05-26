using System.Linq.Expressions;

namespace Eternet.Web.UI.Abstractions.Services;

public abstract class PropertyInlineEditor
{
    public Func<object, object> ViewIdSelector { get; protected set; } = null!; //Func to get the Id from the view
    public string ViewId { get; protected set; } = ""; //Name of the property 'Id' in the view    
    public string ColumnName { get; protected set; } = ""; //Name of the property in the view    
    public Type EntityType { get; protected set; } = null!; //Type of the entity for updating
    public Type ViewType { get; protected set; } = null!;   //Type of the view
    public string EntityId { get; protected set; } = ""; //Name of the property 'Id' in the entity for updating    
    public string PropertyName { get; protected set; } = ""; //Name of the property in the entity for updating
}

public abstract class PropertyInlineEditor<TView, TEntity> : PropertyInlineEditor;

//This class is for cases where the view and the entity have the same Id type and the same property type
public class PropertyInlineEditor<TView, TEntity, TSharedId, TShared> : PropertyInlineEditor<TView, TEntity>
    where TView : notnull
    where TEntity : notnull
{
    public PropertyInlineEditor(
        Expression<Func<TView, TShared>> columnExpression,
        Expression<Func<TView, TSharedId>> viewIdExpression,
        Expression<Func<TEntity, TShared>> propertyExpression,
        Expression<Func<TEntity, TSharedId>> entityIdExpression)
    {
        if (columnExpression.Body is not MemberExpression columnMemberExpression)
        {
            throw new InvalidOperationException("Invalid column expression");
        }
        if (viewIdExpression.Body is not MemberExpression viewIdMemberExpression)
        {
            throw new InvalidOperationException("Invalid viewId expression");
        }
        if (propertyExpression.Body is not MemberExpression propertyMemberExpression)
        {
            throw new InvalidOperationException("Invalid property expression");
        }        
        if (entityIdExpression.Body is not MemberExpression entityIdMemberExpression)
        {
            throw new InvalidOperationException("Invalid entityId expression");
        }

        var column = typeof(TView).GetProperty(columnMemberExpression.Member.Name)
            ?? throw new ArgumentException($"Property {columnMemberExpression.Member.Name} not found");
        ColumnName = column.Name;

        var viewId = typeof(TView).GetProperty(viewIdMemberExpression.Member.Name)
            ?? throw new ArgumentException($"Property {viewIdMemberExpression.Member.Name} not found");
        ViewId = viewId.Name;

        var property = typeof(TEntity).GetProperty(propertyMemberExpression.Member.Name)
                    ?? throw new ArgumentException($"Property {propertyMemberExpression.Member.Name} not found");
        PropertyName = property.Name;

        EntityType = typeof(TEntity);

        ViewType = typeof(TView);

        var entityId = typeof(TEntity).GetProperty(entityIdMemberExpression.Member.Name)
                    ?? throw new ArgumentException($"Property {entityIdMemberExpression.Member.Name} not found");        

        EntityId = entityId.Name;

        ViewIdSelector = CreateExpressionForObject(viewIdExpression).Compile();

    }

    private static Expression<Func<object, object>> CreateExpressionForObject<T, TResult>(Expression<Func<T, TResult>> expression)
    {
        var parameter = Expression.Parameter(typeof(object), "x");
        var body = Expression.Convert(
            Expression.Invoke(expression, Expression.Convert(parameter, typeof(T))),
            typeof(object)
        );
        return Expression.Lambda<Func<object, object>>(body, parameter);
    }
}

namespace Eternet.Web.UI.Abstractions.DataGrid;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FilterableAttribute : Attribute
{
    public Type? FilterType { get; }
    
    public FilterableAttribute() { }

    protected FilterableAttribute(Type filterType)
    {
        FilterType = filterType;
    }
}

public class FilterableAttribute<T> : FilterableAttribute
{
    public FilterableAttribute() : base(typeof(T)) { }
}

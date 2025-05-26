namespace Eternet.Web.UI.Abstractions;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class EternetGridColumnAttribute : Attribute
{
    public string Title { get; set; }
    public bool AllowsSorting { get; set; }
    public bool AllowsSearching { get; init; }
    public bool AllowsFiltering { get; init; }
    public bool AllowsIntegerFiltering { get; init; }
    public bool AutoRender { get; set; }

    public EternetGridColumnAttribute(
        string title = "",
        bool allowsSorting = true,
        bool allowsSearching = true,
        bool allowsFiltering = true,
        bool allowsIntegerFiltering = false,
        bool autoRender = true)
    {
        Title = title;
        AllowsSorting = allowsSorting;
        AllowsSearching = allowsSearching;
        AllowsFiltering = allowsFiltering;
        AllowsIntegerFiltering = allowsIntegerFiltering;
        AutoRender = autoRender;
    }
}

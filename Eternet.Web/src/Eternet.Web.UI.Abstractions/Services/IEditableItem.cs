namespace Eternet.Web.UI.Abstractions.Services;

public interface IEditableItem<T>
{
    bool IsEditing { get; set; }
    T Value { get; set; }
}

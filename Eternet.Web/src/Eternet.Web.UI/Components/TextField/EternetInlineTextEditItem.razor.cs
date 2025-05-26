using Microsoft.AspNetCore.Components.Web;

namespace Eternet.Web.UI;

public partial class EternetInlineTextEditItem<TItem>
{
    [Parameter][EditorRequired] public required TItem Item { get; set; }
    [Parameter] public EventCallback<TItem> ValueChanged { get; set; }
    [Parameter] public EventCallback<TItem> RemoveItemClicked { get; set; }

    private void Save()
    {
        Item.IsEditing = false;
    }

    private void HandleKey(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            Save();
            return;
        }
        if (e.Key == "Escape")
        {
            Item.IsEditing = false;
        }
    }

    private void EditClick()
    {
        Item.IsEditing = true;
    }

}
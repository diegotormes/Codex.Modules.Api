namespace Eternet.Web.UI;

public partial class EternetPaginator
{
    [Parameter, EditorRequired] public required PaginationState State { get; set; }

    private string _itemsPerPageString => State.ItemsPerPage.ToString();

    private async Task HandleItemsPerPageChange(string newValue)
    {
        if (int.TryParse(newValue, out var itemsPerPage))
        {
            if (State.ItemsPerPage != itemsPerPage)
            {
                State.ItemsPerPage = itemsPerPage;
                await State.SetCurrentPageIndexAsync(0).ConfigureAwait(false);
            }
        }
    }
}

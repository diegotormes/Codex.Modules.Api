namespace Eternet.Web.UI.Components.CRUD;
public partial class ListDetailCrudView<TItem> where TItem : class, new()
{
    [Parameter, EditorRequired]
    public required ListDetailService<TItem> CrudService { get; set; }

    [Parameter, EditorRequired]
    public required RenderFragment<TItem> ItemAddEditContent { get; set; }

    [Parameter, EditorRequired]
    public RenderFragment? DataGridContent { get; set; }

    [Parameter]
    public Func<TItem, string, bool>? FilterFunction { get; set; }

    private PaginationState _pagination = new() { ItemsPerPage = 10 };
    private FluentDataGrid<TItem>? _dataGrid;

    private string _searchText = "";

    private IQueryable<TItem> _items = Array.Empty<TItem>().AsQueryable();

    protected override async Task OnInitializedAsync()
    {
        await LoadItems().ConfigureAwait(false);
    }

    private async Task LoadItems()
    {
        _items = (await CrudService.GetAllAsync().ConfigureAwait(false)).AsQueryable();
        StateHasChanged();
    }

    private IQueryable<TItem> FilteredItems
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_searchText) || FilterFunction is null)
            {
                return _items.AsQueryable();
            }

            return _items.Where(item => FilterFunction(item, _searchText)).AsQueryable();
        }
    }

    private void StartAdd()
    {
        CrudService.CurrentItem = new TItem();
        CrudService.IsAdding = true;
    }

    private void StartEdit(TItem item)
    {
        CrudService.CurrentItem = item;
        CrudService.IsEditing = true;
        CrudService.IsAdding = false;
    }

    private async Task OnDeleteItem(TItem item)
    {
        await CrudService.DeleteAsync(item).ConfigureAwait(false);
        await LoadItems().ConfigureAwait(false);
        ResetEdit();
    }

    public async ValueTask ConfirmSaveAsync()
    {
        if (CrudService.CurrentItem is null)
        {
            return;
        }
        if (CrudService.IsAdding)
        {
            await CrudService.CreateAsync(CrudService.CurrentItem).ConfigureAwait(false);
        }
        else
        {
            await CrudService.UpdateAsync(CrudService.CurrentItem).ConfigureAwait(false);
        }
        await LoadItems().ConfigureAwait(false);
        ResetEdit();
    }

    public void ResetEdit()
    {
        CrudService.CurrentItem = null;
        CrudService.IsAdding = false;
        CrudService.IsEditing = false;
    }
}
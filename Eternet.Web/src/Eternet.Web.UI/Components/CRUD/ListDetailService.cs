namespace Eternet.Web.UI.Components.CRUD;

public class ListDetailService<TItem> where TItem : class, new()
{
    public Func<Task<IEnumerable<TItem>>> GetAllAsync { get; set; } = () => Task.FromResult(Array.Empty<TItem>().AsEnumerable());

    public Func<TItem, Task> CreateAsync { get; set; } = _ => Task.CompletedTask;

    public Func<TItem, Task> UpdateAsync { get; set; } = _ => Task.CompletedTask;

    public Func<TItem, Task<bool>> DeleteAsync { get; set; } = _ => Task.FromResult(true);

    public bool IsEditing { get; set; }

    public bool IsAdding { get; set; }

    public TItem? CurrentItem { get; set; }
}

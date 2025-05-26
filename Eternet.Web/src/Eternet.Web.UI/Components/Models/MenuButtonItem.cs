namespace Eternet.Web.UI;

public class MenuButtonItem<TItem>
{
    public bool IsDivider { get; set; }
    public string? Text { get; set; }
    public string? Tooltip { get; set; }
    public Icon? Icon { get; set; }
    public Func<TItem, Task>? OnClick { get; set; }
    public bool IsDisabled { get; set; }
    public bool IsSwitch { get; set; }
    public bool Checked { get; set; }
}

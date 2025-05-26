using Microsoft.AspNetCore.Components.Web;

namespace Eternet.Web.UI;

public partial class EternetMenuButton<TItem> : FluentComponentBase
{
    private static readonly Icon s_defaultIcon = new Icons.Regular.Size24.ChevronDown();

    private bool _visible;
    private Icon? _icon;

    [Parameter]
    public string? Text { get; set; }

    [Parameter]
    [EditorRequired]
    public required TItem Context { get; set; }

    [Parameter]
    public Icon? Icon { get; set; }

    [Parameter]
    [EditorRequired]
    public required IList<MenuButtonItem<TItem>> Items { get; set; }

    [Parameter]
    public Appearance? ButtonAppearance { get; set; }

    public string MenuButtonId { get; } = Identifier.NewId();

    protected override void OnParametersSet()
    {
        _icon = Icon ?? s_defaultIcon;
    }

    private void ToggleMenu()
    {
        _visible = !_visible;
    }

    private async Task HandleItemClicked(MenuButtonItem<TItem> item)
    {
        if (item.OnClick is { } onClick)
        {
            await onClick(Context).ConfigureAwait(false);
        }
        _visible = false;
    }

    private void OnKeyDown(KeyboardEventArgs args)
    {
        if (args is not null && args.Key == "Escape")
        {
            _visible = false;
        }
    }
}

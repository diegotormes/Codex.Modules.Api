using System.Diagnostics;

namespace Eternet.Web.UI;

public partial class EternetButton
{
    [Parameter] public string ComponentId { get; set; } = Guid.NewGuid().ToString();
    [Parameter] public int? TooltipDelay { get; set; } = 200;
    [Parameter] public TimeSpan ClickDelayThreshold { get; set; } = TimeSpan.FromMilliseconds(250);
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public Icon? IconStart { get; set; }
    [Parameter] public Icon? IconEnd { get; set; }
    [Parameter] public string TooltipText { get; set; } = "";
    [Parameter] public string AriaLabel { get; set; } = "";
    [Parameter] public string Style { get; set; } = "";
    [Parameter] public bool Disabled { get => _disabled; set => _disabled = value; }
    [Parameter] public Appearance? Appearance { get; set; }

    private bool _disabled;
    private bool _running;

    private int GetIconSize => (int?)IconStart?.Size ?? 24;
    private string GetIconSizeInPixels() => $"{GetIconSize}px";

    public async Task Click()
    {
        if (!_disabled)
        {
            try
            {
                _disabled = true;
                _running = true;
                await InvokeAsync(StateHasChanged).ConfigureAwait(false);
                var sw = Stopwatch.StartNew();
                await OnClick.InvokeAsync().ConfigureAwait(false);
                sw.Stop();
                if (sw.Elapsed < ClickDelayThreshold)
                {
                    await Task.Delay(ClickDelayThreshold - sw.Elapsed).ConfigureAwait(false);
                }
            }
            finally
            {
                _disabled = false;
                _running = false;
                await InvokeAsync(StateHasChanged).ConfigureAwait(false);
            }
        }
    }

}
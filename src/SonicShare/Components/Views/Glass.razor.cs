using Microsoft.AspNetCore.Components;

namespace SonicShare.Components.Views;

public partial class Glass
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string BackgroundColor { get; set; } = "rgba(255,255,255,0.06)";

    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Parameter]
    public int Blur { get; set; } = 20;

    [Parameter]
    public string Class { get; set; } = string.Empty;

    [Parameter]
    public string Border { get; set; } = "1px solid rgba(255,255,255,0.1)";

    [Parameter]
    public EventCallback Clicked { get; set; }

    private string GetGlassStyle()
    {
        return $@"
                    background-color: {BackgroundColor};
                    border: {Border};
                    transform: translateZ(0);
                    backdrop-filter: blur({Blur}px);
                    ";
    }

    protected override void OnParametersSet()
    {
        StateHasChanged();
    }
}
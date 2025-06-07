using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using SonicShare.Models;

namespace SonicShare.Components.Layout;

public partial class NavMenu
{

    [Parameter]
    public RenderFragment? ChildContent { get; set; }


    private readonly List<TabPage> Tabs = new List<TabPage>
    {
        new TabPage("share","/","Home"),
        new TabPage("list","/search","History"),
        new TabPage("disc","/like","Discover"),
        new TabPage("person","/account","Me"),
    };

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    private void GoToPage(string tabLink)
    {
        NavManager.NavigateTo(tabLink);
    }
}
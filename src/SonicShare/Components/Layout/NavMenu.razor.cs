using Microsoft.AspNetCore.Components;
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
        new TabPage("disc","/logViewer","Logs"),
        new TabPage("person","/account","Me"),
    };


    private void GoToPage(string tabLink)
    {
        NavManager.NavigateTo(tabLink);
    }
}
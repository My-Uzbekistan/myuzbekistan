using Client.Core.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.Pages.Merchant;

public partial class _Form
{
    [Inject] UInjector Injector { get; set; } = null!;

    [Parameter]
    public MerchantView Model { get; set; } = new();

    [Parameter]
    public bool IsNew { get; set; } = false;

    [Parameter]
    public EventCallback<MerchantView> OnSubmit { get; set; }

    [Parameter]
    public bool Processing { get; set; }

    private async void OnValidSubmit(EditContext context)
    {
        await OnSubmit.InvokeAsync(Model);
    }
   
    private string GetTitle() => IsNew ? L["Create"] : L["Edit"] ;
}
namespace Client.Pages.ESimPackage;

public partial class _Form
{
    [Inject] UInjector Injector { get; set; } = null!;

    [Parameter]
    public ESimPackageView Model { get; set; } = new();

    [Parameter]
    public bool IsNew { get; set; } = false;

    [Parameter]
    public EventCallback<ESimPackageView> OnSubmit { get; set; }

    [Parameter]
    public bool Processing { get; set; }

    private async void OnValidSubmit(EditContext context)
    {
        await OnSubmit.InvokeAsync(Model);
    }

    private string GetTitle() => IsNew ? L["Create"] : L["Edit"];
}
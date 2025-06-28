namespace Client.Pages.SimCountry;

public partial class _Form
{
    [Inject] UInjector Injector { get; set; } = null!;

    [Parameter]
    public SimCountryView Model { get; set; } = new();

    [Parameter]
    public bool IsNew { get; set; } = false;

    [Parameter]
    public EventCallback<SimCountryView> OnSubmit { get; set; }

    [Parameter]
    public bool Processing { get; set; }

    private async void OnValidSubmit(EditContext context)
    {
        await OnSubmit.InvokeAsync(Model);
    }
   
    private string GetTitle() => IsNew ? L["Create"] : L["Edit"] ;
}
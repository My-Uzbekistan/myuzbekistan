namespace Client.Pages.ESimPromoCode;

public partial class _Form
{
    [Inject] UInjector Injector { get; set; } = null!;

    [Parameter]
    public ESimPromoCodeView Model { get; set; } = new();

    [Parameter]
    public bool IsNew { get; set; } = false;

    [Parameter]
    public EventCallback<ESimPromoCodeView> OnSubmit { get; set; }

    [Parameter]
    public bool Processing { get; set; }

    private async void OnValidSubmit(EditContext context)
    {
        await OnSubmit.InvokeAsync(Model);
    }

    private void GeneratePromoCode()
    {
        // Generate a random promo code format is "XXXXXXXX"
        var random = new Random();
        var promoCode = new char[8];
        for (int i = 0; i < promoCode.Length; i++)
        {
            promoCode[i] = (char)('A' + random.Next(0, 26)); // Generate a letter A-Z
        }
        Model.Code = new string(promoCode);
    }

    private string GetTitle() => IsNew ? L["Create"] : L["Edit"];
}
namespace Client.Pages.ESimPromoCode;

public partial class Create
{
    [Inject] UInjector Injector { get; set; } = null!;

    public bool Processing { get; set; } = false;

    public async Task OnSubmit(ESimPromoCodeView entity)
    {
        Processing = true;
        var response = await Injector.Commander.Run(new CreateESimPromoCodeCommand(Injector.Session, entity));
        if (response.HasError)
        {
            Injector.Snackbar.Add(L["Error"] + " : " + response.Error?.Message, Severity.Error);
            Processing = false;
            return;
        }
        else
        {
            Injector.Snackbar.Add(L["SuccessCreate"], Severity.Success);
            Injector.NavigationManager.NavigateTo("/esim-promo-codes");
        }
        Processing = false;
    }
}
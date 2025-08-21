namespace Client.Pages.ESimPromoCode;

public partial class Update : ComputedStateComponent<ESimPromoCodeView>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] IESimPromoCodeService ESimPromoCodeService { get; set; } = null!;

    [Parameter]
    public long Id { get; set; }

    public bool Processing { get; set; } = false;

    public async Task OnSubmit(ESimPromoCodeView entity)
    {
        Processing = true;

        var response = await Injector.Commander.Run(new UpdateESimPromoCodeCommand(Injector.Session, entity));
        if (response.HasError)
        {
            Injector.Snackbar.Add(L["Error"] + " : " + response.Error?.Message, Severity.Success);
        }
        else
        {
            Injector.Snackbar.Add(L["SuccessUpdate"], Severity.Success);
            Injector.NavigationManager.NavigateTo("/esim-promo-codes");
        }

        Processing = false;
    }

    protected override async Task<ESimPromoCodeView> ComputeState(CancellationToken cancellationToken)
    {
        return await ESimPromoCodeService.Get(Id, cancellationToken);
    }
}
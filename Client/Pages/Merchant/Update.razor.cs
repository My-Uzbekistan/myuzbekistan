namespace Client.Pages.Merchant;

public partial class Update : ComputedStateComponent<MerchantView>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] IMerchantService MerchantService { get; set; } = null!;
    [Inject] IMerchantCategoryService merchantCategoryService { get; set; } = null!;
    [Parameter] public long MerchantCategoryId { get; set; }

    [Parameter]
    public long Id { get; set; }

    public bool Processing { get; set; } = false;

    public async Task OnSubmit(MerchantView entity)
    {
        Processing = true;
        var merchantCategory = await merchantCategoryService.Get(MerchantCategoryId);
        entity.MerchantCategoryView = merchantCategory;
        var response = await Injector.Commander.Run(new UpdateMerchantCommand(Injector.Session, entity));
        if (response.HasError)
        {
            Injector.Snackbar.Add(L["Error"] + " : " + response.Error?.Message, Severity.Success);
        }
        else
        {
            Injector.Snackbar.Add(L["SuccessUpdate"], Severity.Success);
            Injector.NavigationManager.NavigateTo("/merchants");
        }

        Processing = false;
    }

    protected override async Task<MerchantView> ComputeState(CancellationToken cancellationToken)
    {
        return await MerchantService.Get(Id, cancellationToken);
    }
}
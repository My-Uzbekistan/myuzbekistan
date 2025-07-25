namespace Client.Pages.Merchant;

public partial class Update : ComputedStateComponent<List<MerchantView>>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] IMerchantService MerchantService { get; set; } = null!;
    [Inject] IMerchantCategoryService merchantCategoryService { get; set; } = null!;
    [Parameter] public long MerchantCategoryId { get; set; }

    [Parameter]
    public long Id { get; set; }

    public bool Processing { get; set; } = false;

    public async Task OnSubmit(List<MerchantView> entity)
    {
        Processing = true;
        var merchantCategory = await merchantCategoryService.Get(MerchantCategoryId);
        foreach (var item in entity)
        {
            item.MerchantCategoryView = merchantCategory.First(x => x.Locale == item.Locale);
        }
        var response = await Injector.Commander.Run(new UpdateMerchantCommand(Injector.Session, entity));
        if (response.HasError)
        {
            Injector.Snackbar.Add(L["Error"] + " : " + response.Error?.Message, Severity.Success);
        }
        else
        {
            Injector.Snackbar.Add(L["SuccessUpdate"], Severity.Success);
            Injector.NavigationManager.NavigateTo($"/merchantcategories/{MerchantCategoryId}/merchants");
        }

        Processing = false;
    }

    protected override async Task<List<MerchantView>> ComputeState(CancellationToken cancellationToken)
    {
        return await MerchantService.Get(Id, cancellationToken);
    }
}
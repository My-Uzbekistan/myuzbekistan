namespace Client.Pages.MerchantCategory;

public partial class Update : ComputedStateComponent<List<MerchantCategoryView>>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] IMerchantCategoryService MerchantCategoryService { get; set; } = null!;

    [Parameter]
    public long Id { get; set; }

    public bool Processing { get; set; } = false;

    public async Task OnSubmit(List<MerchantCategoryView> entity)
    {
        Processing = true;

        var response = await Injector.Commander.Run(new UpdateMerchantCategoryCommand(Injector.Session, entity));
        if (response.HasError)
        {
            Injector.Snackbar.Add(L["Error"] + " : " + response.Error?.Message, Severity.Success);
        }
        else
        {
            Injector.Snackbar.Add(L["SuccessUpdate"], Severity.Success);
            Injector.NavigationManager.NavigateTo("/merchantcategories");
        }

        Processing = false;
    }

    protected override async Task<List<MerchantCategoryView>> ComputeState(CancellationToken cancellationToken)
    {
        return await MerchantCategoryService.Get(Id, cancellationToken);
    }
}
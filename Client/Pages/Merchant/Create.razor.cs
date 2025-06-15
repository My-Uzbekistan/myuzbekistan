namespace Client.Pages.Merchant;

public partial class Create
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] IMerchantCategoryService merchantCategoryService { get; set; } = null!;
    [Parameter] public long MerchantCategoryId { get; set; }

    public bool Processing { get; set; } = false;


    public async Task OnSubmit(List<MerchantView> entity)
    {
        Processing = true;
        var merchantCategory = await merchantCategoryService.Get(MerchantCategoryId);
        foreach (var item in entity)
        {
            item.MerchantCategoryView = merchantCategory.First(x=>x.Locale == item.Locale);
        }

        var response = await Injector.Commander.Run(new CreateMerchantCommand(Injector.Session, entity));
        if (response.HasError)
        {
            Injector.Snackbar.Add(L["Error"] + " : " + response.Error?.Message, Severity.Error);
            Processing = false;
            return;
        }
        else
        {
            Injector.Snackbar.Add(L["SuccessCreate"], Severity.Success);
            Injector.NavigationManager.NavigateTo($"/merchantcategories/{MerchantCategoryId}/merchants");
        }
        Processing = false;
    }
}
namespace Client.Pages.MerchantCategory;

public partial class MerchantCategoryList : MixedStateComponent<TableResponse<MerchantCategoryView>, TableOptions>
{
    [Inject] private UInjector Injector { get; set; } = null!;
    [Inject] private IMerchantCategoryService MerchantCategoryService { get; set; } = null!;

    private TableResponse<MerchantCategoryView>? Items ;    
    private readonly string[] SortColumns = ["Logo","BrandName","OrganizationName","Description","Inn","AccountNumber","MfO","Contract","Discount","PayDay","ServiceType","Phone","Email","Address","Vat","Status","Merchants","Id",];

    protected override MutableState<TableOptions>.Options GetMutableStateOptions()
    {
        var uri = Injector.NavigationManager.ToAbsoluteUri(Injector.NavigationManager.Uri);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var _initialCount);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("search", out var searchParam);
        _ = int.TryParse(_initialCount, out int count);
        return new() { InitialValue = new TableOptions() { Page = count == 0 ? 1 : count, PageSize = 15, SortLabel = "Id", SortDirection = 1, Search = searchParam, Lang = CultureInfo.CurrentCulture.Name.Split("-").FirstOrDefault("en") } };
    }

    protected override async Task<TableResponse<MerchantCategoryView>> ComputeState(CancellationToken cancellationToken = default)
    {
        var merchantcategories = await MerchantCategoryService.GetAll(MutableState.Value, cancellationToken);
        return merchantcategories;
    }

    private async Task Delete(long Id, CancellationToken cancellationToken = default)
    {
        bool? result = await Injector.DialogService.ShowMessageBox(
            title: L["DeleteConfirmation"],
            message: L["UnDoneDelete"],
            yesText: "Delete!", 
            cancelText: L["Cancel"]
        );
        if (result ?? false)
        {
            await Injector.Commander.Run(new DeleteMerchantCategoryCommand(Injector.Session, Id), cancellationToken);
            Injector.Snackbar.Add(L["SuccessDelete"], Severity.Success);
        }
    }

    private async Task OnTokenSaved(string token, MerchantCategoryView merchant)
    {
        var command = new UpdateMerchantCategoryTokenCommand(Injector.Session, merchant.Id, token);
        await Injector.Commander.Run(command); // ваш метод сохранения
    }
}
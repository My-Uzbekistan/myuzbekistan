namespace Client.Pages.ESimPackage;

public partial class ESimPackageList : MixedStateComponent<TableResponse<ESimPackageView>, TableOptions>
{
    [Inject] private UInjector Injector { get; set; } = null!;
    [Inject] private IESimPackageService ESimPackageService { get; set; } = null!;

    private TableResponse<ESimPackageView>? Items;
    private readonly string[] SortColumns = 
    [
        "PackageId", "CountryCode", "CountryName", "DataVolume", "ValidDays", 
        "Network", "ActivationPolicy", "Status", "Price", "CustomPrice"
    ];

    protected override MutableState<TableOptions>.Options GetMutableStateOptions()
    {
        var uri = Injector.NavigationManager.ToAbsoluteUri(Injector.NavigationManager.Uri);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var _initialCount);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("search", out var searchParam);
        _ = int.TryParse(_initialCount, out int count);
        return new() { InitialValue = new TableOptions() { Page = count == 0 ? 1 : count, PageSize = 15, SortLabel = "Id", SortDirection = 1, Search = searchParam } };
    }

    protected override async Task<TableResponse<ESimPackageView>> ComputeState(CancellationToken cancellationToken = default)
    {
        var ESimPackages = await ESimPackageService.GetAll(MutableState.Value, cancellationToken);
        return ESimPackages;
    }

    private async Task Delete(long Id, CancellationToken cancellationToken = default)
    {
        bool? result = await Injector.DialogService.ShowMessageBox(
            title: L["DeleteConfirmation"],
            message: L["UnDoneDelete"],
            yesText: "Delete",
            cancelText: L["Cancel"]
        );
        if (result ?? false)
        {
            await Injector.Commander.Run(new DeleteESimPackageCommand(Injector.Session, Id), cancellationToken);
            Injector.Snackbar.Add(L["SuccessDelete"], Severity.Success);
        }
    }
}
namespace Client.Pages.ESimPromoCode;

public partial class ESimPromoCodeList : MixedStateComponent<TableResponse<ESimPromoCodeView>, TableOptions>
{
    [Inject] private UInjector Injector { get; set; } = null!;
    [Inject] private IESimPromoCodeService ESimPromoCodeService { get; set; } = null!;

    private TableResponse<ESimPromoCodeView>? Items;
    private readonly string[] SortColumns = 
    [
        "Id", "Code", "IsCompatibleWithDiscount", "PromoCodeType", "UsageLimit",
        "StartDate", "EndDate", "Status", "DiscountType", "DiscountValue",
        "AppliedCount", "MaxUsagePerUser"
    ];

    private List<string> ColumnWidths = [
        "50px", "150px", "150px", "150px", "150px",
        "150px", "150px", "100px", "150px", "150px",
        "150px", "150px"
    ];
    protected override MutableState<TableOptions>.Options GetMutableStateOptions()
    {
        var uri = Injector.NavigationManager.ToAbsoluteUri(Injector.NavigationManager.Uri);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var _initialCount);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("search", out var searchParam);
        _ = int.TryParse(_initialCount, out int count);
        return new() { InitialValue = new TableOptions() { Page = count == 0 ? 1 : count, PageSize = 15, SortLabel = "Id", SortDirection = 1, Search = searchParam } };
    }

    protected override async Task<TableResponse<ESimPromoCodeView>> ComputeState(CancellationToken cancellationToken = default)
    {
        var ESimPromoCodes = await ESimPromoCodeService.GetAll(MutableState.Value, cancellationToken);
        return ESimPromoCodes;
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
            await Injector.Commander.Run(new DeleteESimPromoCodeCommand(Injector.Session, Id), cancellationToken);
            Injector.Snackbar.Add(L["SuccessDelete"], Severity.Success);
        }
    }
}
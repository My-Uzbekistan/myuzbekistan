namespace Client.Pages.ESimOrder;

public partial class ESimOrder : MixedStateComponent<TableResponse<ESimOrderListView>, TableOptions>
{
    [Inject] private UInjector Injector { get; set; } = null!;
    [Inject] private IESimOrderService ESimOrderService { get; set; } = null!;

    private TableResponse<ESimOrderListView>? Items;

    private readonly string[] SortColumns =
    [
        "PackageId", "User", "CountryName", "DataVolume", "ValidDays",
        "Price", "CustomPrice", "Discount", "PromoCode", "CreatedAt"
    ];

    private readonly List<string> ColumnWidths =
    [
        "250px", "250px", "250px", "150px", "250px",
        "150px", "250px", "200px", "200px", "100px"
    ];

    protected override MutableState<TableOptions>.Options GetMutableStateOptions()
    {
        var uri = Injector.NavigationManager.ToAbsoluteUri(Injector.NavigationManager.Uri);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var _initialCount);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("search", out var searchParam);
        _ = int.TryParse(_initialCount, out int count);
        return new() { InitialValue = new TableOptions() { Page = count == 0 ? 1 : count, PageSize = 15, SortLabel = "Id", SortDirection = 1, Search = searchParam } };
    }

    protected override async Task<TableResponse<ESimOrderListView>> ComputeState(CancellationToken cancellationToken = default)
    {
        var ESimOrders = await ESimOrderService.GetAllList(MutableState.Value, cancellationToken);
        return ESimOrders;
    }
}
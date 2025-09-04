namespace Client.Pages.Device;

public partial class DeviceList : MixedStateComponent<TableResponse<DeviceView>, TableOptions>
{
    [Inject] private UInjector Injector { get; set; } = null!;
    [Inject] private IDeviceService DeviceService { get; set; } = null!;

    private TableResponse<DeviceView>? Items ;    
    private readonly string[] SortColumns = ["UserId","FirebaseToken","OsVersion","Model","AppVersion","Session","Id",];

    protected override MutableState<TableOptions>.Options GetMutableStateOptions()
    {
        var uri = Injector.NavigationManager.ToAbsoluteUri(Injector.NavigationManager.Uri);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var _initialCount);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("search", out var searchParam);
        _ = int.TryParse(_initialCount, out int count);
        return new() { InitialValue = new TableOptions() { Page = count == 0 ? 1 : count, PageSize = 15, SortLabel = "Id", SortDirection = 1, Search = searchParam } };
    }

    protected override async Task<TableResponse<DeviceView>> ComputeState(CancellationToken cancellationToken = default)
    {
        var devices = await DeviceService.GetAll(MutableState.Value, cancellationToken);
        return devices;
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
            await Injector.Commander.Run(new DeleteDeviceCommand(Injector.Session, Id), cancellationToken);
            Injector.Snackbar.Add(L["SuccessDelete"], Severity.Success);
        }
    }
}
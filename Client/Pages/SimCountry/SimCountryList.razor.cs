namespace Client.Pages.SimCountry;

public partial class SimCountryList : MixedStateComponent<TableResponse<SimCountryView>, TableOptions>
{
    [Inject] private UInjector Injector { get; set; } = null!;
    [Inject] private ISimCountryService SimCountryService { get; set; } = null!;

    private TableResponse<SimCountryView>? Items ;    
    private readonly string[] SortColumns = ["Locale","Name","Title","Code","Status","Id",];

    protected override MutableState<TableOptions>.Options GetMutableStateOptions()
    {
        var uri = Injector.NavigationManager.ToAbsoluteUri(Injector.NavigationManager.Uri);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var _initialCount);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("search", out var searchParam);
        _ = int.TryParse(_initialCount, out int count);
        return new() { InitialValue = new TableOptions() { Page = count == 0 ? 1 : count, PageSize = 15, SortLabel = "Id", SortDirection = 1, Search = searchParam } };
    }

    protected override async Task<TableResponse<SimCountryView>> ComputeState(CancellationToken cancellationToken = default)
    {
        var simcountries = await SimCountryService.GetAll(MutableState.Value, cancellationToken);
        return simcountries;
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
            await Injector.Commander.Run(new DeleteSimCountryCommand(Injector.Session, Id), cancellationToken);
            Injector.Snackbar.Add(L["SuccessDelete"], Severity.Success);
        }
    }
}
namespace Client.Pages.CardColor;

public partial class CardColorList : MixedStateComponent<TableResponse<CardColorView>, TableOptions>
{
    [Inject] private UInjector Injector { get; set; } = null!;
    [Inject] private ICardColorService CardColorService { get; set; } = null!;

    private TableResponse<CardColorView>? Items ;    
    private readonly string[] SortColumns = ["Name","ColorCode","Id",];

    protected override MutableState<TableOptions>.Options GetMutableStateOptions()
    {
        var uri = Injector.NavigationManager.ToAbsoluteUri(Injector.NavigationManager.Uri);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var _initialCount);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("search", out var searchParam);
        _ = int.TryParse(_initialCount, out int count);
        return new() { InitialValue = new TableOptions() { Page = count == 0 ? 1 : count, PageSize = 15, SortLabel = "Id", SortDirection = 1, Search = searchParam } };
    }

    protected override async Task<TableResponse<CardColorView>> ComputeState(CancellationToken cancellationToken = default)
    {
        var cardcolors = await CardColorService.GetAll(MutableState.Value, cancellationToken);
        return cardcolors;
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
            await Injector.Commander.Run(new DeleteCardColorCommand(Injector.Session, Id), cancellationToken);
            Injector.Snackbar.Add(L["SuccessDelete"], Severity.Success);
        }
    }
}
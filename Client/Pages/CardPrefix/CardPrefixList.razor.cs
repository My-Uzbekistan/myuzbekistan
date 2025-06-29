namespace Client.Pages.CardPrefix;

public partial class CardPrefixList : MixedStateComponent<TableResponse<CardPrefixView>, TableOptions>
{
    [Inject] private UInjector Injector { get; set; } = null!;
    [Inject] private ICardPrefixService CardPrefixService { get; set; } = null!;

    private TableResponse<CardPrefixView>? Items ;    
    private readonly string[] SortColumns = ["Prefix","BankName","CardType","CardBrand","Id",];

    protected override MutableState<TableOptions>.Options GetMutableStateOptions()
    {
        var uri = Injector.NavigationManager.ToAbsoluteUri(Injector.NavigationManager.Uri);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var _initialCount);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("search", out var searchParam);
        _ = int.TryParse(_initialCount, out int count);
        return new() { InitialValue = new TableOptions() { Page = count == 0 ? 1 : count, PageSize = 15, SortLabel = "Id", SortDirection = 1, Search = searchParam } };
    }

    protected override async Task<TableResponse<CardPrefixView>> ComputeState(CancellationToken cancellationToken = default)
    {
        var cardprefixes = await CardPrefixService.GetAll(MutableState.Value, cancellationToken);
        return cardprefixes;
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
            await Injector.Commander.Run(new DeleteCardPrefixCommand(Injector.Session, Id), cancellationToken);
            Injector.Snackbar.Add(L["SuccessDelete"], Severity.Success);
        }
    }
}
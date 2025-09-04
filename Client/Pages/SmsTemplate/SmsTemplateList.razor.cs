namespace Client.Pages.SmsTemplate;

public partial class SmsTemplateList : MixedStateComponent<TableResponse<SmsTemplateView>, TableOptions>
{
    [Inject] private UInjector Injector { get; set; } = null!;
    [Inject] private ISmsTemplateService SmsTemplateService { get; set; } = null!;

    private TableResponse<SmsTemplateView>? Items ;    
    private readonly string[] SortColumns = ["Locale","Template","Key","Id",];

    protected override MutableState<TableOptions>.Options GetMutableStateOptions()
    {
        var uri = Injector.NavigationManager.ToAbsoluteUri(Injector.NavigationManager.Uri);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var _initialCount);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("search", out var searchParam);
        _ = int.TryParse(_initialCount, out int count);
        return new() { InitialValue = new TableOptions() { Page = count == 0 ? 1 : count, PageSize = 15, SortLabel = "Id", SortDirection = 1, Search = searchParam, Lang = CultureInfo.CurrentCulture.Name.Split("-").FirstOrDefault("en") } };
    }

    protected override async Task<TableResponse<SmsTemplateView>> ComputeState(CancellationToken cancellationToken = default)
    {
        var smstemplates = await SmsTemplateService.GetAll(MutableState.Value, cancellationToken);
        return smstemplates;
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
            await Injector.Commander.Run(new DeleteSmsTemplateCommand(Injector.Session, Id), cancellationToken);
            Injector.Snackbar.Add(L["SuccessDelete"], Severity.Success);
        }
    }
}
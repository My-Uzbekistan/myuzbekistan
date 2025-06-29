namespace Client.Pages.CardPrefix;

public partial class Update : ComputedStateComponent<CardPrefixView>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] ICardPrefixService CardPrefixService { get; set; } = null!;

    [Parameter]
    public long Id { get; set; }

    public bool Processing { get; set; } = false;

    public async Task OnSubmit(CardPrefixView entity)
    {
        Processing = true;

        var response = await Injector.Commander.Run(new UpdateCardPrefixCommand(Injector.Session, entity));
        if (response.HasError)
        {
            Injector.Snackbar.Add(L["Error"] + " : " + response.Error?.Message, Severity.Success);
        }
        else
        {
            Injector.Snackbar.Add(L["SuccessUpdate"], Severity.Success);
            Injector.NavigationManager.NavigateTo("/cardprefixes");
        }

        Processing = false;
    }

    protected override async Task<CardPrefixView> ComputeState(CancellationToken cancellationToken)
    {
        return await CardPrefixService.Get(Id, cancellationToken);
    }
}
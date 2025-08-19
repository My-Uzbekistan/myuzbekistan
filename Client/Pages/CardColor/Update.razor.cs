namespace Client.Pages.CardColor;

public partial class Update : ComputedStateComponent<CardColorView>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] ICardColorService CardColorService { get; set; } = null!;

    [Parameter]
    public long Id { get; set; }

    public bool Processing { get; set; } = false;

    public async Task OnSubmit(CardColorView entity)
    {
        Processing = true;

        var response = await Injector.Commander.Run(new UpdateCardColorCommand(Injector.Session, entity));
        if (response.HasError)
        {
            Injector.Snackbar.Add(L["Error"] + " : " + response.Error?.Message, Severity.Success);
        }
        else
        {
            Injector.Snackbar.Add(L["SuccessUpdate"], Severity.Success);
            Injector.NavigationManager.NavigateTo("/card-colors");
        }

        Processing = false;
    }

    protected override async Task<CardColorView> ComputeState(CancellationToken cancellationToken)
    {
        return await CardColorService.Get(Id, cancellationToken);
    }
}
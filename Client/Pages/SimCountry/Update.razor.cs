namespace Client.Pages.SimCountry;

public partial class Update : ComputedStateComponent<SimCountryView>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] ISimCountryService SimCountryService { get; set; } = null!;

    [Parameter]
    public long Id { get; set; }

    public bool Processing { get; set; } = false;

    public async Task OnSubmit(SimCountryView entity)
    {
        Processing = true;

        var response = await Injector.Commander.Run(new UpdateSimCountryCommand(Injector.Session, entity));
        if (response.HasError)
        {
            Injector.Snackbar.Add(L["Error"] + " : " + response.Error?.Message, Severity.Success);
        }
        else
        {
            Injector.Snackbar.Add(L["SuccessUpdate"], Severity.Success);
            Injector.NavigationManager.NavigateTo("/simcountries");
        }

        Processing = false;
    }

    protected override async Task<SimCountryView> ComputeState(CancellationToken cancellationToken)
    {
        return await SimCountryService.Get(Id, cancellationToken);
    }
}
namespace Client.Pages.CardPrefix;

public partial class Create
{
    [Inject] UInjector Injector { get; set; } = null!;

    public bool Processing { get; set; } = false;

    public async Task OnSubmit(CardPrefixView entity)
    {
        Processing = true;
        var response = await Injector.Commander.Run(new CreateCardPrefixCommand(Injector.Session, entity));
        if (response.HasError)
        {
            Injector.Snackbar.Add(L["Error"] + " : " + response.Error?.Message, Severity.Error);
            Processing = false;
            return;
        }
        else
        {
            Injector.Snackbar.Add(L["SuccessCreate"], Severity.Success);
            Injector.NavigationManager.NavigateTo("/cardprefixes");
        }
        Processing = false;
    }
}
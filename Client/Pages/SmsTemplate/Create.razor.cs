namespace Client.Pages.SmsTemplate;

public partial class Create
{
    [Inject] UInjector Injector { get; set; } = null!;

    public bool Processing { get; set; } = false;

    public async Task OnSubmit(List<SmsTemplateView> entity)
    {
        Processing = true;
        var response = await Injector.Commander.Run(new CreateSmsTemplateCommand(Injector.Session, entity));
        if (response.HasError)
        {
            Injector.Snackbar.Add(L["Error"] + " : " + response.Error?.Message, Severity.Error);
            Processing = false;
            return;
        }
        else
        {
            Injector.Snackbar.Add(L["SuccessCreate"], Severity.Success);
            Injector.NavigationManager.NavigateTo("/sms-templates");
        }
        Processing = false;
    }
}
namespace Client.Pages.SmsTemplate;

public partial class Update : ComputedStateComponent<List<SmsTemplateView>>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] ISmsTemplateService SmsTemplateService { get; set; } = null!;

    [Parameter]
    public long Id { get; set; }

    public bool Processing { get; set; } = false;

    public async Task OnSubmit(List<SmsTemplateView> entity)
    {
        Processing = true;

        var response = await Injector.Commander.Run(new UpdateSmsTemplateCommand(Injector.Session, entity));
        if (response.HasError)
        {
            Injector.Snackbar.Add(L["Error"] + " : " + response.Error?.Message, Severity.Success);
        }
        else
        {
            Injector.Snackbar.Add(L["SuccessUpdate"], Severity.Success);
            Injector.NavigationManager.NavigateTo("/sms-templates");
        }

        Processing = false;
    }

    protected override async Task<List<SmsTemplateView>> ComputeState(CancellationToken cancellationToken)
    {
        return await SmsTemplateService.Get(Id, cancellationToken);
    }
}
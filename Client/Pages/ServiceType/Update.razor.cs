namespace Client.Pages.ServiceType;

public partial class Update : ComputedStateComponent<List<ServiceTypeView>>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] IServiceTypeService ServiceTypeService { get; set; } = null!;

    [Parameter]
    public long Id { get; set; }

    public bool Processing { get; set; } = false;

    public async Task OnSubmit(List<ServiceTypeView> entity)
    {
        Processing = true;

        var response = await Injector.Commander.Run(new UpdateServiceTypeCommand(Injector.Session, entity));
        if (response.HasError)
        {
            Injector.Snackbar.Add(L["Error"] + " : " + response.Error?.Message, Severity.Success);
        }
        else
        {
            Injector.Snackbar.Add(L["SuccessUpdate"], Severity.Success);
            Injector.NavigationManager.NavigateTo("/service-types");
        }

        Processing = false;
    }

    protected override async Task<List<ServiceTypeView>> ComputeState(CancellationToken cancellationToken)
    {
        return await ServiceTypeService.Get(Id, cancellationToken);
    }
}
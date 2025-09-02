namespace Client.Pages.Device;

public partial class Update : ComputedStateComponent<DeviceView>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] IDeviceService DeviceService { get; set; } = null!;

    [Parameter]
    public long Id { get; set; }

    public bool Processing { get; set; } = false;

    public async Task OnSubmit(DeviceView entity)
    {
        Processing = true;

        var response = await Injector.Commander.Run(new UpdateDeviceCommand(Injector.Session, entity));
        if (response.HasError)
        {
            Injector.Snackbar.Add(L["Error"] + " : " + response.Error?.Message, Severity.Success);
        }
        else
        {
            Injector.Snackbar.Add(L["SuccessUpdate"], Severity.Success);
            Injector.NavigationManager.NavigateTo("/devices");
        }

        Processing = false;
    }

    protected override async Task<DeviceView> ComputeState(CancellationToken cancellationToken)
    {
        return await DeviceService.Get(Id, cancellationToken);
    }
}
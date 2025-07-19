namespace Client.Pages.ESimPackage;

public partial class Update : ComputedStateComponent<ESimPackageView>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] IESimPackageService ESimPackageService { get; set; } = null!;

    [Parameter]
    public long Id { get; set; }

    public bool Processing { get; set; } = false;

    public async Task OnSubmit(ESimPackageView entity)
    {
        Processing = true;

        var response = await Injector.Commander.Run(new UpdateESimPackageCommand(entity));
        if (response.HasError)
        {
            Injector.Snackbar.Add(L["Error"] + " : " + response.Error?.Message, Severity.Success);
        }
        else
        {
            Injector.Snackbar.Add(L["SuccessUpdate"], Severity.Success);
            Injector.NavigationManager.NavigateTo("/esim-packages");
        }

        Processing = false;
    }

    protected override async Task<ESimPackageView> ComputeState(CancellationToken cancellationToken)
    {
        return await ESimPackageService.Get(Id, cancellationToken);
    }
}
namespace Client.Pages.AppUser;

public partial class Detail : ComputedStateComponent<UserView>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] IESimPackageService ESimPackageService { get; set; } = null!;

    [Parameter] public long Id { get; set; }

    protected override async Task<UserView> ComputeState(CancellationToken cancellationToken)
    {
        var model = await ESimPackageService.GetUserAsync(Id, cancellationToken);
        return model;
    }
}
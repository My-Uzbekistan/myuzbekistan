namespace Client.Pages.AppUser;

public partial class AppUserList : MixedStateComponent<TableResponse<UserView>, TableOptions>
{
    [Inject] IUserService UserService { get; set; } = null!;
    [Inject] IESimPackageService PackageService { get; set; } = null!;
    [Inject] UInjector Injector { get; set; } = null!;

    private bool _visible;
    private string? _role;
    private TableResponse<UserView>? Items;
    private List<TitleCountModel> TitleCounts = [];
    private readonly string[] SortColumns = ["Id", "FullName", "Email", "Balance"];

    protected override MutableState<TableOptions>.Options GetMutableStateOptions()
    {
        var uri = Injector.NavigationManager.ToAbsoluteUri(Injector.NavigationManager.Uri);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var _initialCount);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("search", out var searchParam);
        _ = int.TryParse(_initialCount, out int count);
        return new() { InitialValue = new TableOptions() { Page = count == 0 ? 1 : count, PageSize = 15, SortLabel = "Id", SortDirection = 1, Search = searchParam } };
    }

    protected override async Task<TableResponse<UserView>> ComputeState(CancellationToken cancellationToken = default)
    {
        var counts = await PackageService.GetCounts(cancellationToken);
        TitleCounts =
        [
            new() { Title = L["TotalPackages"], Count = counts.PackagesCount, Linkable = true, Link = "/esim-packages" },
            new() { Title = L["TotalUsers"], Count = counts.UsersCount },
            new() { Title = L["TotalCountries"], Count = counts.CountriesCount },
        ];
        var users = await UserService.GetAllUsers(MutableState.Value, cancellationToken);
        return users;
    }
}
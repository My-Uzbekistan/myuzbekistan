namespace Client.Pages.AppUser;

public partial class AppUserListTable
{
    [Parameter]
    public RenderFragment<UserView> ChildContent { get; set; } = null!;

    [Parameter]
    public TableResponse<UserView> Values { get; set; } = null!;

    [Parameter]
    public string[] SortColumns { get; set; } = null!;

    [Parameter]
    public IMutableState<TableOptions> MutableState { get; set; } = null!;

    [Parameter]
    public RenderFragment? HeadContent { get; set; }

    [Parameter]
    public List<TitleCountModel> TitleCounts { get; set; } = [];

    [Inject] UInjector Injector { get; set; } = null!;

    private void PageChanged(int page)
    {
        MutableState.Value.Page = page;
        InvalidState();
    }

    private void InvalidState()
    {
        Injector.PageHistoryState.SetPage(MutableState);
        MutableState.Invalidate();
    }

    int Count() => (Values.TotalItems + MutableState.Value.PageSize - 1) / MutableState.Value.PageSize;
}

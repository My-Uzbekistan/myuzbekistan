namespace Client.Pages.Merchant;

public partial class _MerchantMerchantCategoriesSelect : MixedStateComponent<TableResponse<MerchantCategoryView>, string>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] IMerchantCategoryService MerchantCategoryService { get; set; } = null!;

    [Parameter]
    public MerchantCategoryView? Value { get; set; }

    [Parameter]
    public EventCallback<MerchantCategoryView> ValueChanged { get; set; }

    [Parameter]
    public bool Required { get; set; } = false;

    [Parameter]
    public Expression<Func<MerchantCategoryView>>? For { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? Attributes { get; set; }

    private async Task UpdateValue(MerchantCategoryView value)
    {
        Value = value;

        await ValueChanged.InvokeAsync(Value);
        await InvokeAsync(() => StateHasChanged());
    }
    protected override MutableState<string>.Options GetMutableStateOptions()
        => new() { InitialValue = null! };

    protected override async Task<TableResponse<MerchantCategoryView>> ComputeState(CancellationToken cancellationToken)
    {
        return await MerchantCategoryService.GetAll(new() { PageSize = 100 }, cancellationToken);
    }

    private async Task<IEnumerable<MerchantCategoryView>> Search(string value, CancellationToken cancellationToken = default)
    {
        MutableState.Set(value);
        MutableState.Invalidate();

        await Task.Delay(30);

        return State?.GetValue(Injector).Items ?? [];
    }
}
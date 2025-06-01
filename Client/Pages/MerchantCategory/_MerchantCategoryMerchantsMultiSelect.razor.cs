namespace Client.Pages.MerchantCategory;

public partial class _MerchantCategoryMerchantsMultiSelect : ComputedStateComponent<TableResponse<MerchantView>>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] IMerchantService MerchantService { get; set; } = null!;

    [Parameter]
    public String? Label { get; set; }

    [Parameter]
    public ICollection<MerchantView>? Value { get; set; }

    [Parameter]
    public EventCallback<ICollection<MerchantView>> ValueChanged { get; set; }

    [Parameter]
    public bool Required { get; set; } = false;

    [Parameter]
    public Expression<Func<ICollection<MerchantView>>>? For { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? Attributes { get; set; }

    private async Task UpdateValue(ICollection<MerchantView> value)
    {
        Value = value;

        await ValueChanged.InvokeAsync(Value);
        await InvokeAsync(() => StateHasChanged());
    }

    protected override async Task<TableResponse<MerchantView>> ComputeState(CancellationToken cancellationToken)
    {
        return await MerchantService.GetAll(null, new() { PageSize = 100 }, cancellationToken);
    }
}
namespace Client.Pages.MerchantCategory;

public partial class _ServiceTypeSelect : MixedStateComponent<TableResponse<ServiceTypeView>, string>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] IServiceTypeService ServiceTypeService { get; set; } = null!;

    [Parameter]
    public ServiceTypeView? Value { get; set; }

    [Parameter]
    public string? Label { get; set; }

    [Parameter]
    public EventCallback<ServiceTypeView> ValueChanged { get; set; }

    [Parameter]
    public bool Required { get; set; } = false;

    [Parameter]
    public Expression<Func<ServiceTypeView>>? For { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? Attributes { get; set; }

    private async Task UpdateValue(ServiceTypeView value)
    {
        Value = value;

        await ValueChanged.InvokeAsync(Value);
        await InvokeAsync(() => StateHasChanged());
    }
    protected override MutableState<string>.Options GetMutableStateOptions()
        => new() { InitialValue = null! };

    protected override async Task<TableResponse<ServiceTypeView>> ComputeState(CancellationToken cancellationToken)
    {
        return await ServiceTypeService.GetAll(new() { PageSize = 100, Lang = CultureInfo.CurrentCulture.Name.Split("-").FirstOrDefault("en") }, cancellationToken);
    }

    private async Task<IEnumerable<ServiceTypeView>> Search(string value, CancellationToken cancellationToken = default)
    {
        MutableState.Set(value);
        MutableState.Invalidate();

        await Task.Delay(30, cancellationToken);

        return State?.GetValue(Injector).Items ?? [];
    }
}
using ActualLab.Fusion;
using ActualLab.Fusion.Blazor;
using Client.Core.Services;
using Microsoft.AspNetCore.Components;
using myuzbekistan.Shared;
using System.Linq.Expressions;

namespace Client.Pages.Merchant;

public partial class _MerchantMerchantsSelect : MixedStateComponent<TableResponse<MerchantView>, string>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] IMerchantService MerchantService { get; set; } = null!;

    [Parameter]
    public MerchantView? Value { get; set; }

    [Parameter]
    public EventCallback<MerchantView> ValueChanged { get; set; }

    [Parameter]
    public bool Required { get; set; } = false;

    [Parameter]
    public Expression<Func<MerchantView>>? For { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? Attributes { get; set; }

    private async Task UpdateValue(MerchantView value)
    {
        Value = value;

        await ValueChanged.InvokeAsync(Value);
        await InvokeAsync(() => StateHasChanged());
    }
    protected override MutableState<string>.Options GetMutableStateOptions()
        => new() { InitialValue = null! };

    protected override async Task<TableResponse<MerchantView>> ComputeState(CancellationToken cancellationToken)
    {
        return await MerchantService.GetAll(new() { PageSize = 100 }, cancellationToken);
    }

    private async Task<IEnumerable<MerchantView>> Search(string value, CancellationToken cancellationToken = default)
    {
        MutableState.Set(value);
        MutableState.Invalidate();

        await Task.Delay(30);

        return State?.GetValue(Injector).Items ?? [];
    }
}
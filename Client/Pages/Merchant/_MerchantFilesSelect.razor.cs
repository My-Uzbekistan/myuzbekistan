using ActualLab.Fusion;
using ActualLab.Fusion.Blazor;
using Client.Core.Services;
using Microsoft.AspNetCore.Components;
using myuzbekistan.Shared;
using System.Linq.Expressions;

namespace Client.Pages.Merchant;

public partial class _MerchantFilesSelect : MixedStateComponent<TableResponse<FileView>, string>
{
    [Inject] UInjector Injector { get; set; } = null!;
    [Inject] IFileService FileService { get; set; } = null!;

    [Parameter]
    public FileView? Value { get; set; }

    [Parameter]
    public EventCallback<FileView> ValueChanged { get; set; }

    [Parameter]
    public bool Required { get; set; } = false;

    [Parameter]
    public Expression<Func<FileView>>? For { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? Attributes { get; set; }

    private async Task UpdateValue(FileView value)
    {
        Value = value;

        await ValueChanged.InvokeAsync(Value);
        await InvokeAsync(() => StateHasChanged());
    }
    protected override MutableState<string>.Options GetMutableStateOptions()
        => new() { InitialValue = null! };

    protected override async Task<TableResponse<FileView>> ComputeState(CancellationToken cancellationToken)
    {
        return await FileService.GetAll(new() { PageSize = 100 }, cancellationToken);
    }

    private async Task<IEnumerable<FileView>> Search(string value, CancellationToken cancellationToken = default)
    {
        MutableState.Set(value);
        MutableState.Invalidate();

        await Task.Delay(30);

        return State?.GetValue(Injector).Items ?? [];
    }
}
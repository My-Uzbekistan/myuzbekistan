using ActualLab.Fusion;
using ActualLab.Fusion.Blazor;
using Client.Core.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using myuzbekistan.Shared;

namespace Client.Pages.Merchant;

public partial class MerchantList : MixedStateComponent<TableResponse<MerchantView>, TableOptions>
{
    [Inject] private UInjector Injector { get; set; } = null!;
    [Inject] private IMerchantService MerchantService { get; set; } = null!;

    private TableResponse<MerchantView>? Items ;    
    private readonly string[] SortColumns = ["Name","Phone","Email","Address","Description","Contract","Inn","Mfi","CurrentAccount","Discount","IsVat","Image","IsActive","Parent","Id",];

    protected override MutableState<TableOptions>.Options GetMutableStateOptions()
    {
        var uri = Injector.NavigationManager.ToAbsoluteUri(Injector.NavigationManager.Uri);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var _initialCount);
        QueryHelpers.ParseQuery(uri.Query).TryGetValue("search", out var searchParam);
        _ = int.TryParse(_initialCount, out int count);
        return new() { InitialValue = new TableOptions() { Page = count == 0 ? 1 : count, PageSize = 15, SortLabel = "Id", SortDirection = 1, Search = searchParam } };
    }

    protected override async Task<TableResponse<MerchantView>> ComputeState(CancellationToken cancellationToken = default)
    {
        var merchants = await MerchantService.GetAll(MutableState.Value, cancellationToken);
        return merchants;
    }

    private async Task Delete(long Id, CancellationToken cancellationToken = default)
    {
        bool? result = await Injector.DialogService.ShowMessageBox(
            title: L["DeleteConfirmation"],
            message: L["UnDoneDelete"],
            yesText: "Delete!", 
            cancelText: L["Cancel"]
        );
        if (result ?? false)
        {
            await Injector.Commander.Run(new DeleteMerchantCommand(Injector.Session, Id), cancellationToken);
            Injector.Snackbar.Add(L["SuccessDelete"], Severity.Success);
        }
    }
}
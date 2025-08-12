namespace Client.Pages.ESimPackage;

public partial class SetProfitDialog
{
    [Parameter]
    public double percent { get; set; } = 0;
    [Parameter]
    public PackageDiscountView PackageDiscountView { get; set; } = new();
    [CascadingParameter]
    private IMudDialogInstance? MudDialog { get; set; }

    [Inject]
    public UInjector Injector { get; set; } = null!;

    private bool isLoading = false;

    private async Task Submit()
    {
        if (percent < 0 || percent > 100)
        {
            Injector.Snackbar.Add("Profit percentage must be between 0 and 100", Severity.Error);
            return;
        }
        isLoading = true;
        StateHasChanged();
        var countResult = await Injector.Commander.Run(new SetProfitESimPackageCommand(Injector.Session, percent));
        if (!countResult.HasError)
        {
            Injector.Snackbar.Add($"{L["SuccessSetProfit"]} {countResult.Value}", Severity.Success);
            MudDialog!.Close(DialogResult.Ok(true));
        }
        else
        {
            Injector.Snackbar.Add("Failed to update discount: " + countResult.Error?.Message, Severity.Error);
        }

        isLoading = false;
        StateHasChanged();
        MudDialog!.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog!.Cancel();
}
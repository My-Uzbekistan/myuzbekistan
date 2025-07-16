namespace Client.Pages.ESimPackage;

public partial class DiscountDialog
{
    [Parameter]
    public ESimPackageView ESimPackageView { get; set; } = null!;
    [Parameter]
    public PackageDiscountView PackageDiscountView { get; set; } = new();

    [CascadingParameter]
    private IMudDialogInstance? MudDialog { get; set; }

    private void Submit() => MudDialog!.Close(DialogResult.Ok(true));

    private void Cancel() => MudDialog!.Cancel();
}
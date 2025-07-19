namespace Client.Pages.ESimPackage;

public partial class DiscountDialog
{
    [Parameter]
    public ESimPackageView ESimPackageView { get; set; } = null!;
    [Parameter]
    public PackageDiscountView PackageDiscountView { get; set; } = new();
    [CascadingParameter]
    private IMudDialogInstance? MudDialog { get; set; }

    [Inject]
    public UInjector Injector { get; set; } = null!;

    protected override void OnParametersSet()
    {
        if (ESimPackageView.PackageDiscountId > 0)
        {
            PackageDiscountView = ESimPackageView.PackageDiscountView;
        }
        base.OnParametersSet();
    }

    private async Task Submit()
    {
        ESimPackageView.PackageDiscountView = PackageDiscountView;
        var result = await Injector.Commander.Run(new UpdatePackageDiscountCommand(Injector.Session, ESimPackageView));
        if (!result.HasError)
        {
            Injector.Snackbar.Add("Discount updated successfully", Severity.Success);
            MudDialog!.Close(DialogResult.Ok(true));
        }
        else
        {
            Injector.Snackbar.Add("Failed to update discount: " + result.Error?.Message, Severity.Error);
        }
    }

    private void Cancel() => MudDialog!.Cancel();

    private void DiscountPercentageChanged(double discountPercentage)
    {
        if (discountPercentage < 0 || discountPercentage > 100)
        {
            Injector.Snackbar.Add("Discount percentage must be between 0 and 100", Severity.Error);
            return;
        }

        PackageDiscountView.DiscountPercentage = discountPercentage;
        PackageDiscountView.DiscountPrice = ESimPackageView.CustomPrice * ((100 - discountPercentage) / 100);
    }

    private void DiscountPriceChanged(double discountPrice)
    {
        if (discountPrice < 0 || discountPrice > ESimPackageView.CustomPrice)
        {
            Injector.Snackbar.Add("Discount price must be between 0 and the original price", Severity.Error);
            return;
        }
        PackageDiscountView.DiscountPrice = discountPrice;
        PackageDiscountView.DiscountPercentage = ((ESimPackageView.CustomPrice - discountPrice) / ESimPackageView.CustomPrice) * 100;
    }
}
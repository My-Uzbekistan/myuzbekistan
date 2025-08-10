using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;
using static System.Runtime.InteropServices.JavaScript.JSType;

[Mapper]
public static partial class InvoiceMapper
{
    #region Usable
    public static InvoiceView MapToView(this InvoiceEntity src) => src.To();
    public static List<InvoiceView> MapToViewList(this List<InvoiceEntity> src) => src.ToList();
    public static InvoiceEntity MapFromView(this InvoiceView src) => src.From();

    public static List<InvoiceSummaryView> MapToSummaryList(this List<InvoiceEntity> src) => src.Select(MapToSummary).ToList();

    public static InvoiceSummaryView MapToSummary(this InvoiceEntity invoice) => new InvoiceSummaryView
    {
        Merchant = new MerchantData
        {
            Icon = invoice.Merchant.Logo!= null ? ($"{Constants.MinioPath}{invoice.Merchant.Logo.Path}").Replace("//", "/") : null,
            Name = invoice.Merchant.Name,
            Type = invoice.Merchant.MerchantCategory.ServiceType.Name,

        },
        Date = invoice.CreatedAt,
        PaymentId = invoice.ExternalId,
        Amount = (int)(invoice.Amount / 100)
    };

    public static InvoiceDetailView MapToDetail(this InvoiceEntity invoice) => new InvoiceDetailView
    {
        Merchant = new MerchantData
        {
            Icon = invoice.Merchant.Logo != null ? ($"{Constants.MinioPath}{invoice.Merchant.Logo.Path}").Replace("//", "/") : null,
            Name = invoice.Merchant.Name,
            Type = invoice.Merchant.MerchantCategory.ServiceType.Name,

        },
        Date = invoice.CreatedAt,
        PaymentId = invoice.ExternalId,
        Amount = (int)(invoice.Amount / 100)
    };

    #endregion

    #region Internal


    [UserMapping(Default = true)]
    [MapProperty("Merchant", "MerchantView")]
    [MapProperty("Merchant.MerchantCategory", "MerchantView.MerchantCategoryView")]
    [MapProperty("Merchant.MerchantCategory.Logo", "MerchantView.MerchantCategoryView.LogoView")]
    [MapProperty("Merchant.Logo", "MerchantView.LogoView")]
    [MapProperty("Merchant.MerchantCategory.ServiceType", "MerchantView.MerchantCategoryView.ServiceType")]
    private static partial InvoiceView To(this InvoiceEntity src);
    private static partial List<InvoiceView> ToList(this List<InvoiceEntity> src);
    [MapProperty("MerchantView", "Merchant")]
    private static partial InvoiceEntity From(this InvoiceView InvoiceView);
    [MapProperty("MerchantView", "Merchant")]
    public static partial void From(InvoiceView personView, InvoiceEntity personEntity);

    #endregion
}
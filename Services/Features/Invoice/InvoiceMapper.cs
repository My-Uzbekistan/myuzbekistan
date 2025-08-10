[Mapper]
public static partial class InvoiceMapper 
{
    #region Usable
    public static InvoiceView MapToView(this InvoiceEntity src) => src.To();
    public static List<InvoiceView> MapToViewList(this List<InvoiceEntity> src)=> src.ToList();
    public static InvoiceEntity MapFromView(this InvoiceView src) => src.From();
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
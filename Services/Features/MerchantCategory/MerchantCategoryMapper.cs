[Mapper]
public static partial class MerchantCategoryMapper 
{
    #region Usable
    public static MerchantCategoryView MapToView(this MerchantCategoryEntity src) => src.To();
    public static List<MerchantCategoryView> MapToViewList(this List<MerchantCategoryEntity> src)=> src.ToList();
    public static MerchantCategoryEntity MapFromView(this MerchantCategoryView src) => src.From();
    #endregion

    #region Internal
    [UserMapping(Default = true)]
    [MapProperty("Logo", "LogoView")]
    [MapProperty("Merchants", "MerchantsView")]
    private static partial MerchantCategoryView To(this MerchantCategoryEntity src);
    private static partial List<MerchantCategoryView> ToList(this List<MerchantCategoryEntity> src);
    [MapProperty("LogoView", "Logo")]
    [MapProperty("MerchantsView", "Merchants")]
    private static partial MerchantCategoryEntity From(this MerchantCategoryView MerchantCategoryView);
    [MapProperty("LogoView", "Logo")]
    [MapProperty("MerchantsView", "Merchants")]
    public static partial void From(MerchantCategoryView personView, MerchantCategoryEntity personEntity);

    #endregion
}
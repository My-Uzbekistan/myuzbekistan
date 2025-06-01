[Mapper]
public static partial class MerchantMapper 
{
    #region Usable
    public static MerchantView MapToView(this MerchantEntity src) => src.To();
    public static List<MerchantView> MapToViewList(this List<MerchantEntity> src)=> src.ToList();
    public static MerchantEntity MapFromView(this MerchantView src) => src.From();
    #endregion

    #region Internal
    [UserMapping(Default = true)]
    [MapProperty("Logo", "LogoView")]
    [MapProperty("MerchantCategory", "MerchantCategoryView")]
    private static partial MerchantView To(this MerchantEntity src);
    private static partial List<MerchantView> ToList(this List<MerchantEntity> src);
    [MapProperty("LogoView", "Logo")]
    [MapProperty("MerchantCategoryView", "MerchantCategory")]
    private static partial MerchantEntity From(this MerchantView MerchantView);
    [MapProperty("LogoView", "Logo")]
    [MapProperty("MerchantCategoryView", "MerchantCategory")]
    public static partial void From(MerchantView personView, MerchantEntity personEntity);

    #endregion
}
using myuzbekistan.Shared;
using Riok.Mapperly.Abstractions;

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
    [MapProperty("Image", "ImageView")]
    [MapProperty("Parent", "ParentView")]
    private static partial MerchantView To(this MerchantEntity src);
    private static partial List<MerchantView> ToList(this List<MerchantEntity> src);
    [MapProperty("ImageView", "Image")]
    [MapProperty("ParentView", "Parent")]
    private static partial MerchantEntity From(this MerchantView MerchantView);
    [MapProperty("ImageView", "Image")]
    [MapProperty("ParentView", "Parent")]
    public static partial void From(MerchantView personView, MerchantEntity personEntity);

    #endregion
}
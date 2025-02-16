using Riok.Mapperly.Abstractions;
using myuzbekistan.Shared;

namespace myuzbekistan.Services;

[Mapper]
public static partial class FacilityMapper 
{
    #region Usable
    public static FacilityView MapToView(this FacilityEntity src) => src.To();
    public static List<FacilityView> MapToViewList(this List<FacilityEntity> src)=> src.ToList();
    public static FacilityEntity MapFromView(this FacilityView src) => src.From();
    #endregion

    #region Internal

    [MapProperty("Icon", "IconView")]
    private static partial FacilityView To(this FacilityEntity src);
    private static partial List<FacilityView> ToList(this List<FacilityEntity> src);
    [MapProperty("IconView", "Icon")]
    private static partial FacilityEntity From(this FacilityView FacilityView);
    [MapProperty("IconView", "Icon")]
    public static partial void From(FacilityView personView, FacilityEntity personEntity);

    #endregion
}
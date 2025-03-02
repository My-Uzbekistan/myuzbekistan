using Riok.Mapperly.Abstractions;
using myuzbekistan.Shared;

namespace myuzbekistan.Services;

[Mapper]
public static partial class RegionMapper 
{
    #region Usable
    public static RegionView MapToView(this RegionEntity src) => src.To();
    public static RegionApi MapToApi(this RegionEntity src) => src.ToApi();
    public static List<RegionApi> MapToApiList(this List<RegionEntity> src)=> src.ToApiList();
    public static List<RegionView> MapToViewList(this List<RegionEntity> src)=> src.ToList();
    public static RegionEntity MapFromView(this RegionView src) => src.From();
    #endregion

    #region Internal

    [MapProperty("ParentRegion", "ParentRegionView")]
    private static partial RegionView To(this RegionEntity src);
    private static partial RegionApi ToApi(this RegionEntity src);
    private static partial List<RegionApi> ToApiList(this List<RegionEntity> src);
    private static partial List<RegionView> ToList(this List<RegionEntity> src);
    [MapProperty("ParentRegionView", "ParentRegion")]
    private static partial RegionEntity From(this RegionView RegionView);
    [MapProperty("ParentRegionView", "ParentRegion")]
    public static partial void From(RegionView personView, RegionEntity personEntity);

    #endregion
}
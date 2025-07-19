namespace myuzbekistan.Services;

[Mapper]
public static partial class ESimPackageMapper
{
    #region Usable
    public static ESimPackageView MapToView(this ESimPackageEntity src) => src.To();
    public static List<ESimPackageView> MapToViewList(this List<ESimPackageEntity> src) => src.ToList();
    public static ESimPackageEntity MapFromView(this ESimPackageView src) => src.From();
    #endregion

    #region Internal

    private static partial ESimPackageView To(this ESimPackageEntity src);
    private static partial List<ESimPackageView> ToList(this List<ESimPackageEntity> src);
    private static partial ESimPackageEntity From(this ESimPackageView ESimPackageView);
    public static partial void From(ESimPackageView personView, ESimPackageEntity personEntity);

    #endregion
}
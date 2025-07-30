namespace myuzbekistan.Services;

[Mapper]
public static partial class ESimSlugMapper
{
    #region Usable
    public static ESimSlugView MapToView(this ESimSlugEntity src) => src.To();
    public static List<ESimSlugView> MapToViewList(this List<ESimSlugEntity> src) => src.ToList();
    public static ESimSlugEntity MapFromView(this ESimSlugView src) => src.From();
    #endregion

    #region Internal
    [UserMapping(Default = true)]
    private static partial ESimSlugView To(this ESimSlugEntity src);
    private static partial List<ESimSlugView> ToList(this List<ESimSlugEntity> src);
    private static partial ESimSlugEntity From(this ESimSlugView ESimSlugView);
    public static partial void From(ESimSlugView personView, ESimSlugEntity personEntity);

    #endregion
}
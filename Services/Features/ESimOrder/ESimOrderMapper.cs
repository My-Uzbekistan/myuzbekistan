namespace myuzbekistan.Shared;

[Mapper]
public static partial class ESimOrderMapper
{
    #region Usable
    public static ESimOrderView MapToView(this ESimOrderEntity src) => src.To();
    public static List<ESimOrderView> MapToViewList(this List<ESimOrderEntity> src) => src.ToList();
    public static ESimOrderEntity MapFromView(this ESimOrderView src) => src.From();
    #endregion

    #region Internal
    [UserMapping(Default = true)]
    private static partial ESimOrderView To(this ESimOrderEntity src);
    private static partial List<ESimOrderView> ToList(this List<ESimOrderEntity> src);
    private static partial ESimOrderEntity From(this ESimOrderView ESimOrderView);
    public static partial void From(ESimOrderView personView, ESimOrderEntity personEntity);

    #endregion
}
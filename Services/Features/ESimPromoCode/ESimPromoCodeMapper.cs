namespace myuzbekistan.Shared;

[Mapper]
public static partial class ESimPromoCodeMapper
{
    #region Usable
    public static ESimPromoCodeView MapToView(this ESimPromoCodeEntity src) => src.To();
    public static List<ESimPromoCodeView> MapToViewList(this List<ESimPromoCodeEntity> src) => src.ToList();
    public static ESimPromoCodeEntity MapFromView(this ESimPromoCodeView src) => src.From();
    #endregion

    #region Internal
    [UserMapping(Default = true)]
    private static partial ESimPromoCodeView To(this ESimPromoCodeEntity src);
    private static partial List<ESimPromoCodeView> ToList(this List<ESimPromoCodeEntity> src);
    private static partial ESimPromoCodeEntity From(this ESimPromoCodeView ESimPromoCodeView);
    public static partial void From(ESimPromoCodeView personView, ESimPromoCodeEntity personEntity);

    #endregion
}
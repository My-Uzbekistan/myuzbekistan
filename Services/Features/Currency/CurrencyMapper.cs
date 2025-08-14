namespace myuzbekistan.Services;

[Mapper]
public static partial class CurrencyMapper
{
    #region Usable
    public static Currency MapToView(this CurrencyRaw src) => src.To();
    public static List<Currency> MapToViewList(this List<CurrencyRaw> src) => src.ToList();
    #endregion

    #region Internal

    private static partial Currency To(this CurrencyRaw src);
    private static partial List<Currency> ToList(this List<CurrencyRaw> src);

    #endregion
}
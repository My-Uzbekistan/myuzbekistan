using Riok.Mapperly.Abstractions;
using myuzbekistan.Shared;

namespace myuzbekistan.Services;

[Mapper]
public static partial class LanguageMapper 
{
    #region Usable
    public static LanguageView MapToView(this LanguageEntity src) => src.To();
    public static List<LanguageView> MapToViewList(this List<LanguageEntity> src)=> src.ToList();
    public static LanguageEntity MapFromView(this LanguageView src) => src.From();
    #endregion

    #region Internal

    private static partial LanguageView To(this LanguageEntity src);
    private static partial List<LanguageView> ToList(this List<LanguageEntity> src);
    private static partial LanguageEntity From(this LanguageView LanguageView);
    public static partial void From(LanguageView personView, LanguageEntity personEntity);

    #endregion
}
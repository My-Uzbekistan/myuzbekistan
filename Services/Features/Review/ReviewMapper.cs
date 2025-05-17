using Riok.Mapperly.Abstractions;
using myuzbekistan.Shared;

namespace myuzbekistan.Services;

[Mapper]
public static partial class ReviewMapper 
{
    #region Usable
    public static ReviewView MapToView(this ReviewEntity src) => src.To();
    public static List<ReviewView> MapToViewList(this List<ReviewEntity> src)=> src.ToList();
    public static ReviewEntity MapFromView(this ReviewView src) => src.From();
    #endregion

    #region Internal

    private static partial ReviewView To(this ReviewEntity src);
    private static partial List<ReviewView> ToList(this List<ReviewEntity> src);
    private static partial ReviewEntity From(this ReviewView ReviewView);
    public static partial void From(ReviewView personView, ReviewEntity personEntity);

    #endregion
}
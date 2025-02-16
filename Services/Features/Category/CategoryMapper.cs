using Riok.Mapperly.Abstractions;
using myuzbekistan.Shared;

namespace myuzbekistan.Services;

[Mapper]
public static partial class CategoryMapper 
{
    #region Usable
    public static CategoryView MapToView(this CategoryEntity src) => src.To();
    public static List<CategoryView> MapToViewList(this List<CategoryEntity> src)=> src.ToList();
    public static CategoryEntity MapFromView(this CategoryView src) => src.From();
    #endregion

    #region Internal

    [MapProperty("Contents", "ContentsView")]
    [MapProperty("Icon", "IconView")]
    private static partial CategoryView To(this CategoryEntity src);
    private static partial List<CategoryView> ToList(this List<CategoryEntity> src);
    [MapProperty("ContentsView", "Contents")]
    [MapProperty("IconView", "Icon")]
    private static partial CategoryEntity From(this CategoryView CategoryView);
    [MapProperty("ContentsView", "Contents")]
    [MapProperty("IconView", "Icon")]
    public static partial void From(CategoryView personView, CategoryEntity personEntity);

    #endregion
}
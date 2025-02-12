using Riok.Mapperly.Abstractions;
using myuzbekistan.Shared;

namespace myuzbekistan.Services;

[Mapper]
public static partial class FavoriteMapper 
{
    #region Usable
    public static FavoriteView MapToView(this FavoriteEntity src) => src.To();
    public static List<FavoriteView> MapToViewList(this List<FavoriteEntity> src)=> src.ToList();
    public static FavoriteEntity MapFromView(this FavoriteView src) => src.From();
    #endregion

    #region Internal

    [MapProperty("Content", "ContentView")]
    private static partial FavoriteView To(this FavoriteEntity src);
    private static partial List<FavoriteView> ToList(this List<FavoriteEntity> src);
    [MapProperty("ContentView", "Content")]
    private static partial FavoriteEntity From(this FavoriteView FavoriteView);
    [MapProperty("ContentView", "Content")]
    public static partial void From(FavoriteView personView, FavoriteEntity personEntity);

    #endregion
}
using Riok.Mapperly.Abstractions;
using myuzbekistan.Shared;

namespace myuzbekistan.Services;

[Mapper]
public static partial class FavoriteMapper
{
    #region Usable
    public static FavoriteApiView MapToApi(this FavoriteEntity src) => src.ToApi();

    public static List<FavoriteApiView> MapToApiList(this List<FavoriteEntity> src) => src.ToApiList();

    public static FavoriteView MapToView(this FavoriteEntity src) => src.To();
    public static List<FavoriteView> MapToViewList(this List<FavoriteEntity> src) => src.ToList();
    public static FavoriteEntity MapFromView(this FavoriteView src) => src.From();
    #endregion

    #region Internal

    private static List<string> MapToListOfString(ICollection<FileEntity> source)
    {
        var target = new List<string>(source.Count);
        foreach (var item in source)
        {
            target.Add(item.Path!);
        }
        return target;
    }

    private static string? MapToString(FileEntity? source)
    {
        return source?.Path;
    }

    private static double[] MapToGeometryToInt(NetTopologySuite.Geometries.Point source)
    {
        return [source.Coordinate.X, source.Coordinate.Y];
    }

    [MapNestedProperties(nameof(FavoriteEntity.Content))]
    [MapProperty("Id", "FavoriteId")]
    private static partial FavoriteApiView ToApi(this FavoriteEntity src);
    private static partial List<FavoriteApiView> ToApiList(this List<FavoriteEntity> src);

    [MapProperty("Content", "ContentView")]
    private static partial FavoriteView To(this FavoriteEntity src);
    private static partial List<FavoriteView> ToList(this List<FavoriteEntity> src);
    [MapProperty("ContentView", "Content")]
    private static partial FavoriteEntity From(this FavoriteView FavoriteView);
    [MapProperty("ContentView", "Content")]
    public static partial void From(FavoriteView personView, FavoriteEntity personEntity);

    #endregion
}
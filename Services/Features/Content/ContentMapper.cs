using Riok.Mapperly.Abstractions;
using myuzbekistan.Shared;
using NetTopologySuite.Geometries;

namespace myuzbekistan.Services;

[Mapper]
public static partial class ContentMapper
{


    #region Usable

    public static ContentRequestEntity MapToRequest(this ContentRequestView src) => src.MapToRequestEntity();
    public static ContentRequestView MapToRequestV(this ContentRequestEntity src) => src.MapToRequestView();

    public static ContentShort MapToShortApi(this ContentEntity src, bool newApi = false)
    {
        var source = src.ToShortApi();
        if (newApi)
        {
            source.Photo = string.IsNullOrEmpty(source.Photo) ? source.Photo : Constants.MinioPath + source.Photo;
        }

        return source;
    }
    public static MainPageContent MapToApi(this ContentEntity src, bool newApi = false)
    {
        var source = src.ToApi();
        if (src.Category != null)
        {
            source.Caption = src.Category.Caption;
            source.viewType = src.Category.ViewType;
        }

        if (src.Languages != null)
        {
            source.Languages = [.. src.Languages.Select(x => x.Name)];
        }

        if (newApi)
        {
            if (!string.IsNullOrEmpty(source.Photo))
            {
                source.Photo = Constants.MinioPath + source.Photo;
            }

            if (source.Photos != null)
            {
                source.Photos = [.. source.Photos.Where(x => !string.IsNullOrEmpty(x)).Select(x => Constants.MinioPath + x)];
            }
        }

        return source;
    }
    public static ContentView MapToView(this ContentEntity src) => src.To();
    public static List<ContentView> MapToViewList(this List<ContentEntity> src) => src.ToList();
    public static ContentEntity MapFromView(this ContentView src) => src.From();
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

    private static string MapToStringOfRegion(RegionEntity source)
    {
        return source.Name;
    }

    private static List<FacilityItemDto> MapToFacilitiesOfString(List<FacilityEntity> source)
    {
        return source.Select(x => new FacilityItemDto { Name = x.Name, Icon = Constants.MinioPath + x.Icon.Path, Id = x.Id }).ToList();
    }
    private static string MapToString(FileEntity source)
    {
        return source?.Path!;
    }



    private static List<string> MapToLanguage(ICollection<LanguageEntity> source)
    {
        var target = new List<string>(source.Count);
        foreach (var item in source)
        {
            target.Add(item.Name);
        }
        return target;
    }

    private static double[] MapToGeometryToInt(NetTopologySuite.Geometries.Point source)
    {
        return [source.Coordinate.X, source.Coordinate.Y];
    }

    private static partial ContentShort ToShortApi(this ContentEntity src);

    private static partial FacilityApiView MapToFacilityView(FacilityEntity source);

    private static partial ContentRequestEntity MapToRequestEntity(this ContentRequestView src);
    private static partial ContentRequestView MapToRequestView(this ContentRequestEntity src);

    [MapProperty("Id", "ContentId")]
    private static partial MainPageContent ToApi(this ContentEntity src);

    [MapProperty("Category", "CategoryView")]
    [MapProperty("Region", "RegionView")]
    [MapProperty("Facilities", "Facilities")]
    [MapProperty("Contacts", "Contacts")]
    [MapProperty("Files", "FilesView")]
    [MapProperty("Photos", "PhotosView")]
    [MapProperty("Photo", "PhotoView")]
    [MapProperty("Reviews", "ReviewsView")]
    [MapProperty("Languages", "Languages")]
    private static partial ContentView To(this ContentEntity src);

    private static partial List<ContentView> ToList(this List<ContentEntity> src);
    [MapProperty("CategoryView", "Category")]
    [MapProperty("Facilities", "Facilities")]
    [MapProperty("RegionView", "Region")]
    [MapProperty("Contacts", "Contacts")]
    [MapProperty("FilesView", "Files")]
    [MapProperty("PhotosView", "Photos")]
    [MapProperty("ReviewsView", "Reviews")]
    [MapProperty("Languages", "Languages")]
    private static partial ContentEntity From(this ContentView ContentView);
    [MapProperty("CategoryView", "Category")]
    [MapProperty("RegionView", "Region")]
    [MapProperty("Facilities", "Facilities")]
    [MapProperty("Contacts", "Contacts")]
    [MapProperty("FilesView", "Files")]
    [MapProperty("PhotosView", "Photos")]
    [MapProperty("PhotoView", "Photo")]
    [MapProperty("ReviewsView", "Reviews")]
    [MapProperty("Languages", "Languages")]
    public static partial void From(ContentView personView, ContentEntity personEntity);

    #endregion
}
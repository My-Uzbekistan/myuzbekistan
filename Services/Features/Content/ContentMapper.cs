using Riok.Mapperly.Abstractions;
using myuzbekistan.Shared;

namespace myuzbekistan.Services;

[Mapper]
public static partial class ContentMapper 
{
    #region Usable
    public static ContentView MapToView(this ContentEntity src) => src.To();
    public static List<ContentView> MapToViewList(this List<ContentEntity> src)=> src.ToList();
    public static ContentEntity MapFromView(this ContentView src) => src.From();
    #endregion

    #region Internal

    [MapProperty("Category", "CategoryView")]
    [MapProperty("Facilities", "Facilities")]
    [MapProperty("PhoneNumbers", "PhoneNumbers")]
    [MapProperty("Files", "FilesView")]
    [MapProperty("Photos", "PhotosView")]
    [MapProperty("Reviews", "ReviewsView")]
    [MapProperty("Languages", "Languages")]
    private static partial ContentView To(this ContentEntity src);
 
    private static partial List<ContentView> ToList(this List<ContentEntity> src);
    [MapProperty("CategoryView", "Category")]
    [MapProperty("Facilities", "Facilities")]
    [MapProperty("PhoneNumbers", "PhoneNumbers")]
    [MapProperty("FilesView", "Files")]
    [MapProperty("PhotosView", "Photos")]
    [MapProperty("ReviewsView", "Reviews")]
    [MapProperty("Languages", "Languages")]
    private static partial ContentEntity From(this ContentView ContentView);
    [MapProperty("CategoryView", "Category")]
    [MapProperty("Facilities", "Facilities")]
    [MapProperty("PhoneNumbers", "PhoneNumbers")]
    [MapProperty("FilesView", "Files")]
    [MapProperty("PhotosView", "Photos")]
    [MapProperty("ReviewsView", "Reviews")]
    [MapProperty("Languages", "Languages")]
    public static partial void From(ContentView personView, ContentEntity personEntity);

    #endregion
}
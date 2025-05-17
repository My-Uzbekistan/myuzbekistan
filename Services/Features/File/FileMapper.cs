using Riok.Mapperly.Abstractions;
using myuzbekistan.Shared;

namespace myuzbekistan.Services;

[Mapper]
public static partial class FileMapper 
{
    #region Usable
    public static FileView MapToView(this FileEntity src) => src.To();
    public static List<FileView> MapToViewList(this List<FileEntity> src)=> src.ToList();
    public static FileEntity MapFromView(this FileView src) => src.From();
    #endregion

    #region Internal

    private static partial FileView To(this FileEntity src);
    private static partial List<FileView> ToList(this List<FileEntity> src);
    private static partial FileEntity From(this FileView FileView);
    public static partial void From(FileView personView, FileEntity personEntity);

    #endregion
}
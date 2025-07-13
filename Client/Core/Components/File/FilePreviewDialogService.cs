using System.Threading.Tasks;

namespace Client.Core.Components.File;

public static class FilePreviewDialogService
{
    public static async Task ShowFilePreview(this IDialogService dialogService, FileView file)
    {
        var parameters = new DialogParameters
        {
            ["File"] = file
        };

        var options = new DialogOptions
        {
            CloseButton = false,
            NoHeader = true,
            MaxWidth = FileExtensions.IsAudio(file.Name) ||
                       !(FileExtensions.IsMsOfficeFile(file.Name) ||
                         FileExtensions.IsPdf(file.Name) ||
                         FileExtensions.IsTextFile(file.Name) ||
                         FileExtensions.IsImage(file.Name) ||
                         FileExtensions.IsAudio(file.Name) ||
                         FileExtensions.IsVideo(file.Name)) ? MaxWidth.Small : MaxWidth.Large,
            FullWidth = true,
            Position = DialogPosition.Center,
            BackgroundClass = "my-custom-class"
        };

        if (FileExtensions.IsAudio(file.Name))
        {
            parameters.Add("Height", "30vh");
        }

        if (FileExtensions.IsVideo(file.Name))
        {
            parameters.Add("Height", "70vh");
        }

        if (FileExtensions.IsImage(file.Name))
        {
            parameters.Add("Height", "100%");
        }

        await dialogService.ShowAsync<FilePreviewerDialog>(string.Empty, parameters, options);
    }

    public static string ScaleFileName(this string name)
    {
        if (string.IsNullOrEmpty(name))
            return string.Empty;

        if (name.Length < 30)
            return name;

        string[] parts = [name.Substring(0, 17), "...", name.Substring(name.Length - 10, 10)];
        return string.Join("", parts);
    }

    public static string FormatFileSize(this long size)
    {
        if (size < 1024)
            return $"{size} B";
        else if (size < 1024 * 1024)
            return $"{Math.Round((double)size / 1024, 2)} KB";
        else if (size < 1024 * 1024 * 1024)
            return $"{Math.Round((double)size / (1024 * 1024), 2)} MB";
        else
            return $"{Math.Round((double)size / (1024 * 1024 * 1024), 2)} GB";
    }
}
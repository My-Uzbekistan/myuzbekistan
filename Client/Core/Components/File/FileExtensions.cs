/// <summary>
/// Provides helper methods for file type detection and preview URL generation.
/// </summary>
public static class FileExtensions
{
    // Office file extensions
    private static readonly HashSet<string> WordExtensions = [".doc", ".docx", ".dot", ".dotx", ".docm", ".dotm"];
    private static readonly HashSet<string> ExcelExtensions = [".xls", ".xlsx", ".xlsm", ".xlt", ".xltx", ".xltm", ".xlsb"];
    private static readonly HashSet<string> PowerPointExtensions = [".ppt", ".pptx", ".pptm", ".pot", ".potx", ".potm", ".pps", ".ppsx", ".ppsm"];

    // Common file types
    private static readonly HashSet<string> PdfExtensions = [".pdf"];
    private static readonly HashSet<string> VideoExtensions = [".mp4", ".webm", ".ogg", ".avi", ".mov", ".wmv"];
    private static readonly HashSet<string> AudioExtensions = [".mp3", ".wav", ".ogg", ".m4a", ".flac"];
    private static readonly HashSet<string> ImageExtensions = [".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp", ".tiff", ".svg"];

    // Text and code files
    private static readonly HashSet<string> TextExtensions = [".txt", ".md", ".log", ".csv", ".json", ".xml", ".yaml", ".yml", ".html", ".css", ".js", ".cs", ".java", ".py", ".cpp", ".ts"];

    // Archive/compressed files
    private static readonly HashSet<string> ArchiveExtensions = [".zip", ".rar", ".7z", ".tar", ".gz", ".bz2"];

    // Executables
    private static readonly HashSet<string> ExecutableExtensions = [".exe", ".msi", ".apk", ".dmg", ".bin", ".iso"];

    // Design files (limited preview support)
    private static readonly HashSet<string> DesignExtensions = [".psd", ".ai", ".sketch", ".fig"];

    /// <summary>
    /// Determines whether the file is a Microsoft Office file (Word, Excel, or PowerPoint).
    /// </summary>
    public static bool IsMsOfficeFile(string fileName)
    {
        var ext = GetExtension(fileName);
        return WordExtensions.Contains(ext) || ExcelExtensions.Contains(ext) || PowerPointExtensions.Contains(ext);
    }

    public static bool IsWordFile(string fileName) => WordExtensions.Contains(GetExtension(fileName));

    public static bool IsExcelFile(string fileName) => ExcelExtensions.Contains(GetExtension(fileName));

    public static bool IsPresentationFile(string fileName) => PowerPointExtensions.Contains(GetExtension(fileName));

    public static bool IsPdf(string fileName) => PdfExtensions.Contains(GetExtension(fileName));

    public static bool IsVideo(string fileName) => VideoExtensions.Contains(GetExtension(fileName));

    public static bool IsAudio(string fileName) => AudioExtensions.Contains(GetExtension(fileName));

    public static bool IsImage(string fileName) => ImageExtensions.Contains(GetExtension(fileName));

    public static bool IsTextFile(string fileName) => TextExtensions.Contains(GetExtension(fileName));

    public static bool IsArchive(string fileName) => ArchiveExtensions.Contains(GetExtension(fileName));

    public static bool IsExecutable(string fileName) => ExecutableExtensions.Contains(GetExtension(fileName));

    public static bool IsDesignFile(string fileName) => DesignExtensions.Contains(GetExtension(fileName));

    /// <summary>
    /// Returns a category name for a file based on its extension.
    /// </summary>
    /// <param name="fileName">The file name or path.</param>
    /// <returns>A string category such as "Image", "Video", "Text", etc.</returns>
    public static string GetFileCategory(string fileName)
    {
        if (IsImage(fileName)) return "Image";
        if (IsPdf(fileName)) return "PDF";
        if (IsAudio(fileName)) return "Audio";
        if (IsVideo(fileName)) return "Video";
        if (IsWordFile(fileName)) return "Word";
        if (IsExcelFile(fileName)) return "Excel";
        if (IsPresentationFile(fileName)) return "Presentation";
        if (IsTextFile(fileName)) return "Text";
        if (IsArchive(fileName)) return "Archive";
        if (IsExecutable(fileName)) return "Executable";
        if (IsDesignFile(fileName)) return "Design";
        return "Unknown";
    }

    /// <summary>
    /// Returns a preview URL suitable for iframes, image, audio, or video tags.
    /// </summary>
    public static string GetUrl(string fileUrl)
    {
        if (IsWordFile(fileUrl) || IsExcelFile(fileUrl) || IsPresentationFile(fileUrl))
        {
            return $"https://view.officeapps.live.com/op/embed.aspx?src={Uri.EscapeDataString(fileUrl)}";
        }

        if (IsPdf(fileUrl) || IsAudio(fileUrl) || IsVideo(fileUrl) || IsImage(fileUrl))
        {
            return fileUrl;
        }

        // Other types are not previewable in standard UI
        return string.Empty;
    }

    /// <summary>
    /// Gets the lowercase file extension with a leading dot.
    /// Returns an empty string if no extension is found.
    /// </summary>
    private static string GetExtension(string fileName)
    {
        return Path.GetExtension(fileName)?.ToLowerInvariant() ?? string.Empty;
    }

    private static readonly Dictionary<string, string> DefaultIcons = new()
    {
        { "csv", "icons/csv.svg" },
        { "docx", "icons/docx.svg" },
        { "xlsx", "icons/xlsx.svg" },
        { "pptx", "icons/pptx.svg" },

        { "pdf", "icons/pdf.png" },
        { "text", "icons/txt.png" },
        { "archive", "icons/archive.png" },
        { "video", "icons/video.png" },
        { "audio", "icons/audio.png" },
        { "file", "icons/file.png" },
        { "design", "icons/design.png" },
        { "executable", "icons/executable.png" },
    };

    public static string GetPreviewOrIcon(this FileView fileView, string baseUrl)
    {
        string url;
        if (IsPdf(fileView.Name))
        {
            url = DefaultIcons.GetValueOrDefault("pdf") ?? string.Empty;
        }
        else if (IsVideo(fileView.Name))
        {
            url = DefaultIcons.GetValueOrDefault("video") ?? string.Empty;
        }
        else if (IsAudio(fileView.Name))
        {
            url = DefaultIcons.GetValueOrDefault("audio") ?? string.Empty;
        }
        else if (IsWordFile(fileView.Name))
        {
            url = DefaultIcons.GetValueOrDefault("docx") ?? string.Empty;
        }
        else if (IsExcelFile(fileView.Name))
        {
            url = DefaultIcons.GetValueOrDefault("xlsx") ?? string.Empty;
        }
        else if (IsPresentationFile(fileView.Name))
        {
            url = DefaultIcons.GetValueOrDefault("pptx") ?? string.Empty;
        }
        else if (IsArchive(fileView.Name))
        {
            url = DefaultIcons.GetValueOrDefault("archive") ?? string.Empty;
        }
        else if (IsTextFile(fileView.Name))
        {
            url = DefaultIcons.GetValueOrDefault("text") ?? string.Empty;
        }
        else if (IsDesignFile(fileView.Name))
        {
            url = DefaultIcons.GetValueOrDefault("design") ?? string.Empty;
        }
        else if (IsExecutable(fileView.Name))
        {
            url = DefaultIcons.GetValueOrDefault("executable") ?? string.Empty;
        }
        else if (IsImage(fileView.Name))
        {
            url = $"{baseUrl}{fileView.Path}";
        }
        else
        {
            url = DefaultIcons.GetValueOrDefault("file") ?? string.Empty;
        }

        return url;
    }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace myuzbekistan.Shared;

public partial class FileEntity : BaseEntity
{

    public string Name { get; set; } = null!;
    public Guid? FileId { get; set; }
    public string? Extension { get; set; }
    public string? Path { get; set; }
    public long Size { get; set; } = 0;
    public UFileTypes Type { get; set; } = UFileTypes.File;

    public ICollection<ContentEntity>? ContentPhotos { get; set; } = new List<ContentEntity>();
    public ICollection<ContentEntity>? ContentFiles { get; set; } = new List<ContentEntity>();
    public ContentEntity? ContentPhoto { get; set; } 

}

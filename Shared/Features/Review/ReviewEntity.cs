
namespace myuzbekistan.Shared;
[SkipGeneration]
public partial class ReviewEntity : BaseEntity
{
    public long UserId { get; set; }
    public string? Comment { get; set; } = null!;
    public int Rating { get; set; }

    public ContentEntity? ContentEntity { get; set; } = null!;
    public long ContentEntityId { get; set; }
    public string ContentEntityLocale { get; set; } = null!;
}

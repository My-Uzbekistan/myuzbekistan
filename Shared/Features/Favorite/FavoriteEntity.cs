using System.ComponentModel.DataAnnotations.Schema;

namespace myuzbekistan.Shared;
[SkipGeneration]
public partial class FavoriteEntity : BaseEntity
{
    public ContentEntity Content { get; set; } = null!;
    public long ContentId { get; set; }
    public string ContentLocale { get; set; } = null!;
    
    public long UserId { get; set; }

    [NotMapped]
    public ApplicationUser? User { get; set; } = null!;
}

using System.ComponentModel.DataAnnotations.Schema;

namespace myuzbekistan.Shared;

public partial class FavoriteEntity : BaseEntity
{
    public ContentEntity Content { get; set; } = null!;
    public long UserId { get; set; } 
}

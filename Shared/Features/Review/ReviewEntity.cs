
namespace myuzbekistan.Shared;

public partial class ReviewEntity : BaseEntity
{
    public long UserId { get; set; }
    public string Comment { get; set; } = null!;
    public int Rating { get; set; }
}

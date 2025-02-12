namespace myuzbekistan.Shared;

public partial class CategoryEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<ContentEntity>? Contents { get; set; } = [];

}

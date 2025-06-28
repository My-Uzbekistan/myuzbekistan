using Microsoft.EntityFrameworkCore;

namespace myuzbekistan.Shared;

[PrimaryKey(nameof(FacilityEntity.Id), nameof(FacilityEntity.Locale))]
[SkipGeneration]
public class FacilityEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Locale { get; set; } = null!;
    public FileEntity? Icon { get; set; } 

    public ICollection<ContentEntity> Contents { get; set; } = new List<ContentEntity>();

}

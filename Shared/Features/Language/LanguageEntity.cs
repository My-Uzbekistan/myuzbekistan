using Microsoft.EntityFrameworkCore;

namespace myuzbekistan.Shared;

[PrimaryKey(nameof(FacilityEntity.Id), nameof(FacilityEntity.Locale))]
public partial class LanguageEntity:BaseEntity
{
    public string Name { get; set; } = null!;
    public string Locale { get; set; } = null!;

    public ICollection<ContentEntity>? Contents { get; set; }
}


using Microsoft.EntityFrameworkCore;

namespace myuzbekistan.Shared;

[PrimaryKey(nameof(ServiceTypeEntity.Id), nameof(ServiceTypeEntity.Locale))]
public class ServiceTypeEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Locale { get; set; } = null!;
}

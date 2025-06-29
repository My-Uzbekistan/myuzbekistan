using Microsoft.EntityFrameworkCore;

namespace myuzbekistan.Shared;

[PrimaryKey(nameof(ContentEntity.Id), nameof(ContentEntity.Locale))]
public class SimCountryEntity:BaseEntity
{
    public string Locale { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Code { get; set; } = null!;
    public bool Status { get; set; } = false;
}

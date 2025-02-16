using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace myuzbekistan.Shared;



[PrimaryKey(nameof(CategoryEntity.Id), nameof(CategoryEntity.Locale))]
public partial class CategoryEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; } 
    public string Locale { get; set; } = null!;
    public short Order { get; set; } = 0;
    public List<ContentEntity>? Contents { get; set; } = [];
    public FileEntity? Icon { get; set; }

    public ContentStatus Status { get; set; } = ContentStatus.Active;

    public int Fields { get; set; }


}

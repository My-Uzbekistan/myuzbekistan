using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

namespace myuzbekistan.Shared;

[PrimaryKey(nameof(CategoryEntity.Id), nameof(CategoryEntity.Locale))]
public partial class CategoryEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Caption { get; set; } = null!;
    public string? Description { get; set; } 
    public string Locale { get; set; } = null!;
    public short Order { get; set; } = 0;

    public ViewType ViewType { get; set; } = ViewType.Place;

    public List<ContentEntity>? Contents { get; set; } = [];
    public FileEntity? Icon { get; set; }

    public ContentStatus Status { get; set; } = ContentStatus.Active;

    public int Fields { get; set; }

    [Column(TypeName = "jsonb")]
    public Dictionary< ContentFields,string>? FieldNames { get; set; } = [];

}

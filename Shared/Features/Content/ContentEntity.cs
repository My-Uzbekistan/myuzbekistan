using Microsoft.EntityFrameworkCore;
using Shared.Localization;
using System.ComponentModel.DataAnnotations;
using Point = NetTopologySuite.Geometries.Point;

namespace myuzbekistan.Shared;

[PrimaryKey(nameof(CategoryEntity.Id), nameof(CategoryEntity.Locale))]
public class ContentEntity : BaseEntity
{
    
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public long CategoryId { get; set; } 
    public CategoryEntity Category { get; set; } = null!;
     
    public string WorkingHours { get; set; } = null!;
    public string[]? Facilities { get; set; } = [];
    public Point? Location { get; set; }
    public int[]? PhoneNumbers { get; set; } = [];
    public ICollection<FileEntity>? Files { get; set; } = [];
    public ICollection<FileEntity>? Photos { get; set; } = [];
    public ICollection<ReviewEntity>? Reviews { get; set; } = [];
    public string[]? Languages { get; set; } = [];
    public int RatingAverage { get; set; } 
    public decimal Price { get; set; }
    public decimal PriceInDollar { get; set; }
    public string? Address { get; set; } = null!;

    public bool Recommended { get; set; } = false;
    public string Locale { get; set; } = null!;
    public ContentStatus Status { get; set; } = ContentStatus.Active;

}

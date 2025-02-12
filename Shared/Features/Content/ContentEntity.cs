using Point = NetTopologySuite.Geometries.Point;

namespace myuzbekistan.Shared;

public class ContentEntity : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string CategoryId { get; set; } = null!;
    public CategoryEntity Category { get; set; } = null!;
    public bool IsPublished { get; set; }
    public bool IsDeleted { get; set; }
    public string Slug { get; set; } = null!;
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
    public bool Recommended { get; set; }   

}

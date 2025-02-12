using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.Runtime.Serialization;
using Point = NetTopologySuite.Geometries.Point;
namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ContentView
{
   [property: DataMember] public string Title { get; set; } = null!;
   [property: DataMember] public string Description { get; set; } = null!;
   [property: DataMember] public string CategoryId { get; set; } = null!;
   [property: DataMember] public CategoryView CategoryView { get; set; } 
   [property: DataMember] public bool IsPublished { get; set; } 
   [property: DataMember] public bool IsDeleted { get; set; } 
   [property: DataMember] public string Slug { get; set; } = null!;
   [property: DataMember] public string WorkingHours { get; set; } = null!;
   [property: DataMember] public String[]? Facilities { get; set; } = null!;
   [property: DataMember, MemoryPackAllowSerialize] public Point? Location { get; set; } 
   [property: DataMember] public Int32[] PhoneNumbers { get; set; } = null!;
   [property: DataMember] public ICollection<FileView>? FilesView { get; set; } 
   [property: DataMember] public ICollection<FileView>? PhotosView { get; set; } 
   [property: DataMember] public ICollection<ReviewView>? ReviewsView { get; set; } 
   [property: DataMember] public String[]? Languages { get; set; } = null!;
   [property: DataMember] public int RatingAverage { get; set; }
   [property: DataMember] public Decimal Price { get; set; }
   [property: DataMember] public Decimal PriceInDollar { get; set; }
   [property: DataMember] public string? Address { get; set; } 
   [property: DataMember] public bool Recommended { get; set; } 
   [property: DataMember] public long Id { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as ContentView;
        return other?.Id == Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
}

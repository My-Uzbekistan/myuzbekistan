using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.Runtime.Serialization;
using Point = NetTopologySuite.Geometries.Point;
using Shared.Localization;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ContentApiView
{
    [property: DataMember] public string Title { get; set; } = null!;
    [property: DataMember] public string? Description { get; set; }
    [property: DataMember] public long CategoryId { get; set; }
    [property: DataMember] public CategoryView CategoryView { get; set; } = null!;
    [property: DataMember] public string? WorkingHours { get; set; }
    [property: DataMember] public ICollection<FacilityApiView> Facilities { get; set; } = [];
    [property: DataMember] public double[]? Location { get; set; }
    [property: DataMember] public List<CallInformation>? PhoneNumbers { get; set; } = [];
    [property: DataMember] public List<string>? Files { get; set; }
    [property: DataMember] public ICollection<string>? Photos { get; set; }
    [property: DataMember] public string? Photo { get; set; }
    [property: DataMember] public ICollection<string>? Languages { get; set; }
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

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ContentView
{
    [Display(ResourceType = typeof(SharedResource), Name = "Title")]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(SharedResource))]
    [property: DataMember] public string Title { get; set; } = null!;
   [property: DataMember] public string? Description { get; set; } 
   [property: DataMember] public long CategoryId { get; set; }
    [property: DataMember] public CategoryView CategoryView { get; set; } = null!;
   [property: DataMember] public string? Slug { get; set; } 
   [property: DataMember] public string? WorkingHours { get; set; } 
   [property: DataMember] public ICollection<FacilityView> Facilities { get; set; } = [];
   [property: DataMember, MemoryPackAllowSerialize, JsonConverter(typeof(NetTopologySuite.IO.Converters.GeometryConverter))] public Point? Location { get; set; } 
   [property: DataMember] public List<CallInformation>? PhoneNumbers { get; set; } = [];
   [property: DataMember] public ICollection<FileView>? FilesView { get; set; } 
   [property: DataMember] public ICollection<FileView>? PhotosView { get; set; } 
   [property: DataMember] public FileView? PhotoView { get; set; } 
   [property: DataMember] public ICollection<ReviewView>? ReviewsView { get; set; } 
   [property: DataMember] public ICollection<LanguageView>? Languages { get; set; } 
   [property: DataMember] public int RatingAverage { get; set; }
   [property: DataMember] public Decimal Price { get; set; }
   [property: DataMember] public Decimal PriceInDollar { get; set; }
   [property: DataMember] public string? Address { get; set; } 
   [property: DataMember] public bool Recommended { get; set; } 
   [property: DataMember] public long Id { get; set; }
    [property: DataMember] public string Locale { get; set; } = null!;
    [property: DataMember] public ContentStatus Status { get; set; } = ContentStatus.Active;

    public override bool Equals(object? o)
    {
        var other = o as ContentView;
        return other?.Id == Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
}

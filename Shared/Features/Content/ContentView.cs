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
    [property: DataMember] public int AverageCheck { get; set; }
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


[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ContentDto
{
    [property: DataMember] public FieldDto<long> Id { get; set; } = null!;
    [property: DataMember] public FieldDto<string> Title { get; set; } = null!;
    [property: DataMember] public FieldDto<string?> Description { get; set; } = null!;
    [property: DataMember] public FieldDto<string?> Photo { get; set; } = null!;
    [property: DataMember] public FieldDto<List<string>> Files { get; set; } = null!;
    [property: DataMember] public FieldDto<List<string>> Photos { get; set; } = null!;
    [property: DataMember] public FieldDto<List<PhoneNumberDto>> PhoneNumbers { get; set; } = null!;
    [property: DataMember] public FieldDto<List<FacilityItemDto>> Facilities { get; set; } = null!;
    [property: DataMember] public FieldDto<List<LanguageItemDto>> Languages { get; set; } = null!;
}
[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class FieldDto<T>
{
    [property: DataMember] public string Name { get; set; } = null!;
    [property: DataMember] public T Value { get; set; } = default!;
}
[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class FacilitiesDto
{
    [property: DataMember] public string Name { get; set; } = null!;
    [property: DataMember] public List<FacilityItemDto> Value { get; set; } = new();
}
[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class FacilityItemDto
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public string Name { get; set; } = null!;
    [property: DataMember] public string? Icon { get; set; }
}
[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class LanguagesDto
{
    [property: DataMember] public string Name { get; set; } = null!;
    [property: DataMember] public List<LanguageItemDto> Value { get; set; } = new();
}
[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class LanguageItemDto
{
    [property: DataMember] public FieldDto<string?> Id { get; set; } = null!;
    [property: DataMember] public FieldDto<string?> Name { get; set; } = null!;
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PhoneNumberDto
{
    [property: DataMember] public string Icon { get; set; } = "phone.svg"; // По умолчанию иконка телефона
    [property: DataMember] public string Name { get; set; } = "";
    [property: DataMember] public string Contact { get; set; } = null!;
}
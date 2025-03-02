using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Shared.Localization;

namespace myuzbekistan.Shared;
[DataContract, MemoryPackable]
public partial record CategoryApi([property: DataMember] string Name, [property: DataMember] string Icon, [property: DataMember] long Id);
public partial record MainPageApi(
    [property: DataMember] string CategoryName,
    [property: DataMember] long CategoryId,
    [property: DataMember] MainPageContent? Recommended,
    [property: DataMember] ViewType ViewType,
    [property: DataMember] List<MainPageContent> Contents
    );

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class MainPageContent
{
    [property: DataMember] public long ContentId { get; set; }
    [property: DataMember] public string Title { get; set; } = string.Empty;
    [property: DataMember] public string Caption { get; set; } = string.Empty;
    [property: DataMember] public List<string> Photos { get; set; } = [];
    [property: DataMember] public string Photo { get; set; } = string.Empty;
    [property: DataMember] public string Region { get; set; } = string.Empty;
    [property: DataMember] public string Facilities { get; set; } = string.Empty;
    [property: DataMember] public int RatingAverage { get; set; }
    [property: DataMember] public int AverageCheck { get; set; }
    [property: DataMember] public decimal Price { get; set; }
    [property: DataMember] public decimal PriceInDollar { get; set; }
}




[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class CategoryView
{
    [property: DataMember]
    [Display(ResourceType = typeof(SharedResource), Name = "Name")]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(SharedResource))]
    public string Name { get; set; } = null!;
    [property: DataMember] public string Caption { get; set; } = null!;
    [property: DataMember] public string Locale { get; set; } = null!;
    [property: DataMember] public string? Description { get; set; }
    [property: DataMember] public ICollection<ContentView>? ContentsView { get; set; }
    [property: DataMember] public FileView? IconView { get; set; }
    [property: DataMember] public short Order { get; set; } = 0;
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public int Fields { get; set; }
    [property: DataMember] public Dictionary<ContentFields, string> FieldNames { get; set; } = [];

    [property: DataMember] public ViewType ViewType { get; set; } = ViewType.Restaurant;
    [property: DataMember] public ContentStatus Status { get; set; } = ContentStatus.Active;

    public override bool Equals(object? o)
    {
        var other = o as CategoryView;
        return other?.Id == Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
}

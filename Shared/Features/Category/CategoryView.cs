using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Shared.Localization;

namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class CategoryView
{
    [property: DataMember]
    [Display(ResourceType = typeof(SharedResource), Name = "Name")]
    [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(SharedResource))]
    public string Name { get; set; } = null!;
    [property: DataMember] public string Locale { get; set; } = null!;
    [property: DataMember] public string? Description { get; set; }
    [property: DataMember] public ICollection<ContentView>? ContentsView { get; set; }
    [property: DataMember]  public FileView? IconView { get; set; }
    [property: DataMember] public short Order { get; set; } = 0;
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public int Fields { get; set; }
    [property: DataMember] public ContentStatus Status { get; set; } = ContentStatus.Active;

    public override bool Equals(object? o)
    {
        var other = o as CategoryView;
        return other?.Id == Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
}

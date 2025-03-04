using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.Runtime.Serialization;
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
    [property: DataMember] public List<CallInformation>? Contacts { get; set; } = [];
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


public class FieldWrapper<T>
{
    public string Name { get; set; } = null!;
    public T Value { get; set; } = default!;
}
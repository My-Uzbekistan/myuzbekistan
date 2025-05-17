using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.Runtime.Serialization;

namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class RegionView
{
    [property: DataMember] public string Name { get; set; } = null!;
    [property: DataMember] public string Locale { get; set; } = null!;
    [property: DataMember] public bool IsActive { get; set; }
    [property: DataMember] public RegionView? ParentRegionView { get; set; }
    [property: DataMember] public ICollection<ContentView>? ContentsView { get; set; }
    [property: DataMember] public long Id { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as RegionView;
        return other?.Id == Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
}


[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class RegionApi
{
    [property: DataMember] public string Name { get; set; } = null!;
    [property: DataMember] public long Id { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as RegionView;
        return other?.Id == Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
}
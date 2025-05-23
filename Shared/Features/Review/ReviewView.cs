using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.Runtime.Serialization;

namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ReviewView
{
    [property: DataMember] public long UserId { get; set; }
    [property: DataMember] public string? Comment { get; set; } = null!;
    [property: DataMember] public int Rating { get; set; }
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public ContentView? ContentView { get; set; } = null!;
    [property: DataMember] public long ContentEntityId { get; set; }
    [property: DataMember] public string ContentEntityLocale { get; set; } = null!;

    public override bool Equals(object? o)
    {
        var other = o as ReviewView;
        return other?.Id == Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
}

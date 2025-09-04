using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.Runtime.Serialization;
// keep both serializers available globally; disambiguate attributes explicitly

namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ReviewView
{
    [property: DataMember] public long UserId { get; set; }
    [property: DataMember] public string? Comment { get; set; } = null!;
    [property: DataMember] public int Rating { get; set; }
    [property: DataMember] public long Id { get; set; }

    // Hidden navigation / fk fields (not returned in JSON)
    [System.Text.Json.Serialization.JsonIgnore] public ContentView? ContentView { get; set; } = null!;
    [System.Text.Json.Serialization.JsonIgnore] public long ContentEntityId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore] public string ContentEntityLocale { get; set; } = null!;

    // Optional user display info (populate elsewhere if needed)
    [property: DataMember] public string? UserName { get; set; }
    [property: DataMember] public string? Avatar { get; set; }

    public override bool Equals(object? o) => o is ReviewView other && other.Id == Id;
    public override int GetHashCode() => Id.GetHashCode();
}

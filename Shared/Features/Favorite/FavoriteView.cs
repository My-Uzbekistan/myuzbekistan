using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.Runtime.Serialization;

namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class FavoriteApiView: ContentApiView
{
    [property: DataMember] public long FavoriteId { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as FavoriteView;
        return other?.Id == Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class FavoriteView
{
   [property: DataMember] public ContentView ContentView { get; set; } 
   [property: DataMember] public long UserId { get; set; }
    [property: DataMember] public ApplicationUserView? User { get; set; } = null!;
   [property: DataMember] public long Id { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as FavoriteView;
        return other?.Id == Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
}

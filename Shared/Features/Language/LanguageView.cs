using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.Runtime.Serialization;

namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class LanguageView
{
   [property: DataMember] public string Name { get; set; } = null!;
   [property: DataMember] public string Locale { get; set; } = null!;
   [property: DataMember] public long Id { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as LanguageView;
        return other?.Id == Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
}

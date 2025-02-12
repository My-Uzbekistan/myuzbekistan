using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.Runtime.Serialization;

namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class FileView
{
   [property: DataMember] public string Name { get; set; } = null!;
   [property: DataMember] public Guid? FileId { get; set; } 
   [property: DataMember] public string? Extension { get; set; } 
   [property: DataMember] public string? Path { get; set; } 
   [property: DataMember] public long Size { get; set; }
   [property: DataMember] public UFileTypes Type { get; set; } 
   [property: DataMember] public long Id { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as FileView;
        return other?.Id == Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
}

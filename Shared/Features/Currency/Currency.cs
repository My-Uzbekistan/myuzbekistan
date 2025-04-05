using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.Runtime.Serialization;
namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class Currency
{
    [property: DataMember] public int Id { get; set; }
    [property: DataMember] public string Ccy { get; set; } = null!;
    [property: DataMember] public string Rate { get; set; } = null!;
}

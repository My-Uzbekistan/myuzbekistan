using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.Runtime.Serialization;

namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class AuditLogView
{
    [property: DataMember] public DateTime Created { get; set; } = DateTime.Now;

    [property: DataMember] public string TableName { get; set; } = null!;

    [property: DataMember] public string? OldValues { get; set; }

    [property: DataMember] public string? NewValues { get; set; }

    [property: DataMember] public string Operation { get; set; } = null!;

    [property: DataMember] public string User { get; set; } = null!;

}

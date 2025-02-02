using EF.Audit;
using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace myuzbekistan.Shared;

[Auditable]
public sealed partial class TodoEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public long? ImageId { get; set; }

}

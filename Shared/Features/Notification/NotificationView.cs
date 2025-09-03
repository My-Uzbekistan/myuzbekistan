using System.Runtime.Serialization;
using MemoryPack;

namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class NotificationView
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public string Title { get; set; } = null!;
    [property: DataMember] public string? Image { get; set; }
    [property: DataMember] public string Content { get; set; } = null!;
    [property: DataMember] public string? ActionLink { get; set; }
    [property: DataMember] public DateTime PublishAt { get; set; }
    [property: DataMember] public bool IsSeen { get; set; }

    public override bool Equals(object? o) => (o as NotificationView)?.Id == Id;
    public override int GetHashCode() => Id.GetHashCode();
}

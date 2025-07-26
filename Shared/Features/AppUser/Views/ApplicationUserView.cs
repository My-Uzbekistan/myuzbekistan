namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ApplicationUserView
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [property: DataMember] public string UserName { get; set; } = null!;
    [property: DataMember] public string Email { get; set; } = null!;
}
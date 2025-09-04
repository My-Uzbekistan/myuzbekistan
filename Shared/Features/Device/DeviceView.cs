[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class DeviceView
{
    [property: DataMember] public long UserId { get; set; }
    [property: DataMember] public string FirebaseToken { get; set; } = null!;
    [property: DataMember] public string OsVersion { get; set; } = null!;
    [property: DataMember] public string Model { get; set; } = null!;
    [property: DataMember] public string AppVersion { get; set; } = null!;
    [property: DataMember] public string Session { get; set; } = null!;
    [property: DataMember] public long Id { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as DeviceView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
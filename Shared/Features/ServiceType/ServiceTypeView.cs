[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ServiceTypeView
{
    [property: DataMember] public string Name { get; set; } = null!;
    [property: DataMember] public string Locale { get; set; } = null!;
    [property: DataMember] public long Id { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as ServiceTypeView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
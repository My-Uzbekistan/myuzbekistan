[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class CardColorView
{
    [property: DataMember] public string Name { get; set; } = null!;
    [property: DataMember] public string ColorCode { get; set; } = null!;
    [property: DataMember] public long Id { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as CardColorView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class SimCountryView
{
    [property: DataMember] public string Locale { get; set; } = null!;
    [property: DataMember] public string Name { get; set; } = null!;
    [property: DataMember] public string Title { get; set; } = null!;
    [property: DataMember] public string Code { get; set; } = null!;
    [property: DataMember] public bool Status { get; set; } 
    [property: DataMember] public long Id { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as SimCountryView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
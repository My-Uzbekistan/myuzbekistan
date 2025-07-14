namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ESimPackageView
{
    [property: DataMember] public long Id { get; set; }
    // other properties

    public override bool Equals(object? o)
    {
        var other = o as ESimPackageView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
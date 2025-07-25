namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class CreateESimOrderView
{
    [property: DataMember] public string PackageId { get; set; } = string.Empty;

    public override bool Equals(object? o)
    {
        var other = o as CreateESimOrderView;
        return other?.PackageId == PackageId;
    }

    public override int GetHashCode()
        => HashCode.Combine(PackageId);
}
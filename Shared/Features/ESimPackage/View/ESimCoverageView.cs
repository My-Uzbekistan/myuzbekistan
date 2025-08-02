namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ESimCoverageView
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public string Name { get; set; } = string.Empty;
    [property: DataMember] public string Code { get; set; } = string.Empty;
    [property: DataMember] public string ImageUrl { get; set; } = string.Empty;
    [property: DataMember] public List<ESimCoverageNetworkView> Networks { get; set; } = [];

    public override bool Equals(object? o)
    {
        var other = o as ESimCoverageView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}


[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ESimCoverageNetworkView
{
    [property: DataMember] public string Name { get; set; } = string.Empty;
    [property: DataMember] public List<string> Types { get; set; } = [];
}
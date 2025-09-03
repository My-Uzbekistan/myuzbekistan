namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ESimOrderListView
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public DateTime CreatedAt { get; set; }
    [property: DataMember] public UserView User { get; set; } = new();
    [property: DataMember] public ESimPackageView ESimPackageView { get; set; } = new();
    [property: DataMember] public ESimPromoCodeView? ESimPromoCodeView { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as ESimOrderListView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
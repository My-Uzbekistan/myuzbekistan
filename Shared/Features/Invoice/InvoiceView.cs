namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class InvoiceView
{
    [property: DataMember] public Decimal Amount { get; set; }
    [property: DataMember] public string? Currency { get; set; } = "UZS";
    [property: DataMember] public string? Description { get; set; }
    [property: DataMember] public ApplicationUserView? User { get; set; } = null!;
    [property: DataMember] public MerchantView MerchantView { get; set; } = null!;
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public DateTime CreatedAt { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as InvoiceView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}

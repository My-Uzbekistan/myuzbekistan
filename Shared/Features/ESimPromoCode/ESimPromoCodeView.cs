namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ESimPromoCodeView
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public bool IsCompatibleWithDiscount { get; set; } = false;
    [property: DataMember] public string Code { get; set; } = string.Empty;
    [property: DataMember] public PromoCodeType PromoCodeType { get; set; }
    [property: DataMember] public int UsageLimit { get; set; }
    [property: DataMember] public DateTime? StartDate { get; set; }
    [property: DataMember] public DateTime? EndDate { get; set; }
    [property: DataMember] public bool IsActive { get; set; } = true;
    [property: DataMember] public DiscountType DiscountType { get; set; }
    [property: DataMember] public double DiscountValue { get; set; }
    [property: DataMember] public int AppliedCount { get; set; } = 0;
    [property: DataMember] public int MaxUsagePerUser { get; set; } = 1;

    public override bool Equals(object? o)
    {
        var other = o as ESimPromoCodeView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
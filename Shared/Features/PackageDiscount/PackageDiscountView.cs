namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageDiscountView
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public long ESimPackageId { get; set; }
    [property: DataMember] public double DiscountPercentage { get; set; }
    [property: DataMember] public double DiscountPrice { get; set; }
    [property: DataMember] public ContentStatus Status { get; set; }
    [property: DataMember] public DateTime StartDate { get; set; }
    [property: DataMember] public DateTime EndDate { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as PackageDiscountView;
        return other?.Id == Id &&
                other?.ESimPackageId == ESimPackageId &&
                other?.DiscountPercentage == DiscountPercentage &&
                other?.DiscountPrice == DiscountPrice &&
                other?.Status == Status &&
                other?.StartDate == StartDate &&
                other?.EndDate == EndDate;
    }

    public override int GetHashCode()
        => HashCode.Combine(Id, ESimPackageId, DiscountPercentage, DiscountPrice, Status, StartDate, EndDate);
}
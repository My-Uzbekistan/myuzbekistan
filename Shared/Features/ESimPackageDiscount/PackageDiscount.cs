namespace myuzbekistan.Shared;

public class PackageDiscountEntity : BaseEntity
{
    public long ESimPackageId { get; set; }
    public ESimPackageEntity? ESimPackage { get; set; }
    public double DiscountPercentage { get; set; }
    public double DiscountPrice { get; set; }
    public ContentStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public PackageDiscountView MapToView()
        => new()
        {
            Id = Id,
            ESimPackageId = ESimPackageId,
            DiscountPercentage = DiscountPercentage,
            DiscountPrice = DiscountPrice,
            Status = Status,
            StartDate = StartDate,
            EndDate = EndDate
        };
}
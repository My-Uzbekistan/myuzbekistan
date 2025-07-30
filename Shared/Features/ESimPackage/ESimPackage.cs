namespace myuzbekistan.Shared;

public class ESimPackageEntity : BaseEntity
{
    public string OperatorName { get; set; } = string.Empty;
    public string PackageId { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string DataVolume { get; set; } = string.Empty;
    public int ValidDays { get; set; }
    public double Price { get; set; }
    public double CustomPrice { get; set; }
    public string Network { get; set; } = string.Empty;
    public string ActivationPolicy { get; set; } = string.Empty;
    public ContentStatus Status { get; set; } = ContentStatus.Active;
    public long? PackageDiscountId { get; set; }
    public PackageDiscountEntity? PackageDiscountEntity { get; set; }

    public bool? IsRoaming { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
    public List<string>? Info { get; set; } = [];
    public string? OtherInfo { get; set; }
    public List<PackageResponseCoverage>? Coverage { get; set; } = [];
    public bool HasVoicePack { get; set; }

    public long ESimSlugId { get; set; }
    public ESimSlugEntity? ESimSlug { get; set; }
}
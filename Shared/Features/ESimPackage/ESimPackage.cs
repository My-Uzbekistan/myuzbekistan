namespace myuzbekistan.Shared;

public class ESimPackage : BaseEntity
{
    public string PackageId { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public string DataVolume { get; set; } = string.Empty;
    public int ValidDays { get; set; }
    public double Price { get; set; }
    public double CustomPrice { get; set; }
    public string Network { get; set; } = string.Empty;
    public string ActivationPolicy { get; set; } = string.Empty;
}
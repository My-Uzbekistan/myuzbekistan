namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class EsimView
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public string PackageId { get; set; } = string.Empty;
    [property: DataMember] public string CountryName { get; set; } = string.Empty;
    [property: DataMember] public string OperatorName { get; set; } = string.Empty;
    [property: DataMember] public string DataValume { get; set; } = string.Empty;
    [property: DataMember] public double RemainingData { get; set; }
    [property: DataMember] public string? ActivationDate { get; set; } = string.Empty;
    [property: DataMember] public int ValidDays { get; set; }
    [property: DataMember] public string ImageUrl { get; set; } = string.Empty;
    [property: DataMember] public string ICCID { get; set; } = string.Empty;
    [property: DataMember] public string Status { get; set; } = string.Empty;
    [property: DataMember] public List<PackageResponseCoverage> Coverage { get; set; } = [];
    [property: DataMember] public string QrCode { get; set; } = string.Empty;
    [property: DataMember] public string QrCodeUrl { get; set; } = string.Empty;
    [property: DataMember] public string DirectAppleUrl { get; set; } = string.Empty;
    [property: DataMember] public string ManualInstallation { get; set; } = string.Empty;
    [property: DataMember] public string QrCodeInstallation { get; set; } = string.Empty;
    [property: DataMember] public List<ESimPackageView> OtherPackages { get; set; } = [];

    public override bool Equals(object? o)
    {
        var other = o as EsimView;
        return other?.Id == Id &&
                other?.CountrySlug == CountrySlug &&
                other?.CountryName == CountryName &&
                other?.OperatorName == OperatorName &&
                other?.DataValume == DataValume &&
                other?.RemainingData == RemainingData &&
                other?.ActivationDate == ActivationDate &&
                other?.ValidDays == ValidDays &&
                other?.ImageUrl == ImageUrl &&
                other?.ICCID == ICCID &&
                other?.Status == Status;
    }

    public override int GetHashCode()
        => HashCode.Combine(Id, CountrySlug, CountryName, OperatorName, ICCID);
}
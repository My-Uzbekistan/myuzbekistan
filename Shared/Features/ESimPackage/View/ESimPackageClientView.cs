namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ESimPackageClientView
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public PackageDiscountView? PackageDiscountView { get; set; }
    [property: DataMember] public string PackageId { get; set; } = string.Empty;
    [property: DataMember] public string OperatorName { get; set; } = string.Empty;
    [property: DataMember] public string CountryCode { get; set; } = string.Empty;
    [property: DataMember] public string CountryName { get; set; } = string.Empty;
    [property: DataMember] public string DataVolume { get; set; } = string.Empty;
    [property: DataMember] public int ValidDays { get; set; }
    [property: DataMember] public bool HasVoicePack { get; set; }
    [property: DataMember] public int Voice { get; set; }
    [property: DataMember] public int Text { get; set; }
    [property: DataMember] public double CustomPrice { get; set; }
    [property: DataMember] public string Network { get; set; } = string.Empty;
    [property: DataMember] public string ActivationPolicy { get; set; } = string.Empty;
    [property: DataMember] public ContentStatus Status { get; set; }
    [property: DataMember] public bool? IsRoaming { get; set; }
    [property: DataMember] public string? ImageUrl { get; set; } = string.Empty;
    [property: DataMember] public List<string>? Info { get; set; } = [];
    [property: DataMember] public string? OtherInfo { get; set; }
    [property: DataMember] public List<PackageResponseCoverage>? Coverage { get; set; } = [];
    [property: DataMember] public List<ESimSlugView> Countries { get; set; } = [];

    public override bool Equals(object? o)
    {
        var other = o as ESimPackageClientView;
        return other?.Id == Id &&
               other?.PackageId == PackageId &&
               other?.CountryCode == CountryCode &&
               other?.CountryName == CountryName &&
               other?.DataVolume == DataVolume &&
               other?.ValidDays == ValidDays &&
               other?.CustomPrice == CustomPrice &&
               other?.Network == Network &&
               other?.ActivationPolicy == ActivationPolicy;
    }

    public override int GetHashCode()
        => HashCode.Combine(Id, PackageId, CountryCode, CountryName, DataVolume, ValidDays);

    public static implicit operator ESimPackageClientView(ESimPackageView src)
        {
        return new ESimPackageClientView
        {
            Id = src.Id,
            PackageDiscountView = src.PackageDiscountView,
            PackageId = src.PackageId,
            OperatorName = src.OperatorName,
            CountryCode = src.CountryCode,
            CountryName = src.CountryName,
            DataVolume = src.DataVolume,
            Voice = src.Voice,
            Text = src.Text,
            ValidDays = src.ValidDays,
            CustomPrice = src.CustomPrice,
            Network = src.Network,
            ActivationPolicy = src.ActivationPolicy,
            Status = src.Status,
            IsRoaming = src.IsRoaming,
            ImageUrl = src.ImageUrl,
            Info = src.Info,
            OtherInfo = src.OtherInfo,
            Coverage = src.Coverage,
            HasVoicePack = false,
        };
    }
}
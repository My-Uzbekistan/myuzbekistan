namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ESimPackageView
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public long? PackageDiscountId { get; set; }
    [property: DataMember] public PackageDiscountView? PackageDiscountView { get; set; }
    [property: DataMember] public string PackageId { get; set; } = string.Empty;
    [property: DataMember] public string OperatorName { get; set; } = string.Empty;
    [property: DataMember] public string CountryCode { get; set; } = string.Empty;
    [property: DataMember] public string CountryName { get; set; } = string.Empty;
    [property: DataMember] public string DataVolume { get; set; } = string.Empty;
    [property: DataMember] public int ValidDays { get; set; }
    [property: DataMember] public double Price { get; set; }
    [property: DataMember] public double CustomPrice { get; set; }
    [property: DataMember] public string Network { get; set; } = string.Empty;
    [property: DataMember] public string ActivationPolicy { get; set; } = string.Empty;
    [property: DataMember] public ContentStatus Status { get; set; }
    [property: DataMember] public bool? IsRoaming { get; set; }
    [property: DataMember] public string? ImageUrl { get; set; } = string.Empty;
    [property: DataMember] public List<string>? Info { get; set; } = [];
    [property: DataMember] public string? OtherInfo { get; set; }
    [property: DataMember] public List<PackageResponseCoverage>? Coverage { get; set; } = [];

    public override bool Equals(object? o)
    {
        var other = o as ESimPackageView;
        return other?.Id == Id && 
               other?.PackageId == PackageId &&
               other?.CountryCode == CountryCode &&
               other?.CountryName == CountryName &&
               other?.DataVolume == DataVolume &&
               other?.ValidDays == ValidDays &&
               other?.Price == Price &&
               other?.CustomPrice == CustomPrice &&
               other?.Network == Network &&
               other?.ActivationPolicy == ActivationPolicy;
    }

    public override int GetHashCode()
        => HashCode.Combine(Id, PackageId, CountryCode, CountryName, DataVolume, ValidDays, Price);

    public static List<ESimPackageView> FromApiResponse(PackageResponseView src)
    {
        List<ESimPackageView> result = [];
        var firstResponse = src.Data.FirstOrDefault();
        if (firstResponse == null || firstResponse.Operators == null || !firstResponse.Operators.Any())
        {
            return result; // Return empty list if no data is available
        }

        foreach (var provider in firstResponse.Operators)
        {
            foreach(var package in provider.Packages)
            {
                ESimPackageView packageView = new()
                {
                    PackageId = package.Id,
                    CountryCode = firstResponse.CountryCode,
                    CountryName = firstResponse.Title,
                    DataVolume = $"{package.Amount / 1024} GB",
                    ValidDays = package.Day,
                    Price = package.Price,
                    Network = provider.Title,
                    ActivationPolicy = provider.ActivationPolicy,
                    OperatorName = provider.Title,
                    IsRoaming = provider.IsRoaming,
                    ImageUrl = provider.Image.Url,
                    Info = provider.Info,
                    OtherInfo = provider.OtherInfo,
                    Coverage = provider.Coverages
                };
                result.Add(packageView);
            }
        }

        return result;
    }
}
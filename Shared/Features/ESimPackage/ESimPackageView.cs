namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ESimPackageView
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public long? PackageDiscountId { get; set; }
    [property: DataMember] public PackageDiscountView PackageDiscountView { get; set; } = new();
    [property: DataMember] public string PackageId { get; set; } = string.Empty;
    [property: DataMember] public string CountryCode { get; set; } = string.Empty;
    [property: DataMember] public string CountryName { get; set; } = string.Empty;
    [property: DataMember] public string DataVolume { get; set; } = string.Empty;
    [property: DataMember] public int ValidDays { get; set; }
    [property: DataMember] public double Price { get; set; }
    [property: DataMember] public double CustomPrice { get; set; }
    [property: DataMember] public string Network { get; set; } = string.Empty;
    [property: DataMember] public string ActivationPolicy { get; set; } = string.Empty;
    [property: DataMember] public ContentStatus Status { get; set; }

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
                    ActivationPolicy = provider.ActivationPolicy
                };
                result.Add(packageView);
            }
        }

        return result;
    }
}
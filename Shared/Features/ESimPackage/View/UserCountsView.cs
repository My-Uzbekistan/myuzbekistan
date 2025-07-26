namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class UserCountsView
{
    [property: DataMember] public int PackagesCount { get; set; }
    [property: DataMember] public int CountriesCount { get; set; }
    [property: DataMember] public int UsersCount { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as UserCountsView;
        return other is not null &&
               PackagesCount == other.PackagesCount &&
               CountriesCount == other.CountriesCount &&
               UsersCount == other.UsersCount;
    }

    public override int GetHashCode()
        => HashCode.Combine(PackagesCount, CountriesCount, UsersCount);
}
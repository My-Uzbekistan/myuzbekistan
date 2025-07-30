namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ESimSlugView
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public string Slug { get; set; } = string.Empty;
    [property: DataMember] public string Title { get; set; } = string.Empty;
    [property: DataMember] public string? CountryCode { get; set; } = string.Empty;
    [property: DataMember] public string? ImageUrl { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as ESimSlugView;
        return other?.Id == Id &&
                other?.Slug == Slug &&
                other?.Title == Title &&
                other?.CountryCode == CountryCode &&
                other?.ImageUrl == ImageUrl;
    }

    public override int GetHashCode()
        => HashCode.Combine(Id, Slug, Title, CountryCode, ImageUrl);
}
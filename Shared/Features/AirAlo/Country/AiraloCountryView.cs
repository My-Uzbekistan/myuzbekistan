namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class AiraloCountryView
{
    [DataMember, JsonProperty("id")]
    public int Id { get; set; }
    [DataMember, JsonProperty("slug")]
    public string Slug { get; set; } = string.Empty;
    [DataMember, JsonProperty("title")]
    public string Title { get; set; } = string.Empty;
    [DataMember, JsonProperty("image")]
    public AirAloImage Image { get; set; } = new();
    [DataMember, JsonProperty("seo")]
    public int PackageCount { get; set; }

    public static implicit operator AiraloCountryView(AiraloCountryResponseView response)
    {
        return new AiraloCountryView
        {
            Id = response.Id,
            Slug = response.Slug,
            Title = response.Title,
            Image = response.Image,
            PackageCount = response.PackageCount
        };
    }
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class AiraloCountryResponseView
{
    [DataMember, JsonProperty("id")]
    public int Id { get; set; }
    [DataMember, JsonProperty("slug")]
    public string Slug { get; set; } = string.Empty;
    [DataMember, JsonProperty("title")]
    public string Title { get; set; } = string.Empty;
    [DataMember, JsonProperty("image")]
    public AirAloImage Image { get; set; } = new();
    [DataMember, JsonProperty("seo")]
    public AirAloSeo Seo { get; set; } = new();
    [DataMember, JsonProperty("package_count")]
    public int PackageCount { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as AiraloCountryView;
        return other?.Id == Id &&
               other.Slug == Slug &&
               other.Title == Title &&
               other.PackageCount == PackageCount;
    }

    public override int GetHashCode()
        => HashCode.Combine(Id, Slug, Title, Image, Seo, PackageCount);
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class AirAloImage
{
    [JsonProperty("width")]
    public int Width { get; set; }
    [JsonProperty("height")]
    public int Height { get; set; }
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class AirAloSeo
{
    [JsonProperty("title")]
    public string? Title { get; set; }
    [JsonProperty("keywords")]
    public List<string> Keywords { get; set; } = [];
    [JsonProperty("description")]
    public string? Description { get; set; }
}
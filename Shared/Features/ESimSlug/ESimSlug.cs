namespace myuzbekistan.Shared;

public class ESimSlugEntity : BaseEntity
{
    public string Slug { get; set; } = string.Empty;
    public string TitleUz { get; set; } = string.Empty;
    public string TitleRu { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string? CountryCode { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public ESimSlugType SlugType { get; set; }

    public ESimSlugView ToView(Language language)
        => new()
        {
            Id = Id,
            Slug = Slug,
            CountryCode = CountryCode,
            Title = language switch
            {
                Language.ru => TitleRu,
                Language.en => TitleEn,
                _ => TitleUz
            },
            ImageUrl = ImageUrl
        };
}
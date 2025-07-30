namespace myuzbekistan.Shared;

public enum Language
{
    uz,
    ru,
    en,
}

public static class LanguageExtensions
{
    public static Language ConvertToLanguage(this string? language)
        => language switch
        {
            "ru" => Language.ru,
            "en" => Language.en,
            _ => Language.uz
        };
}
namespace myuzbekistan.Services;

public class AiraloCountryService(IConfiguration configuration) : IAiraloCountryService
{
    private readonly JsonSerializerSettings jsonSerializerSettings = new()
    {
        ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Ignore
    };

    #region Queries

    public virtual async Task<List<AiraloCountryView>> GetAllAsync(Language language, CancellationToken cancellationToken = default)
    {
        using var _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(language.ToString());

        var response = await _httpClient.GetAsync($"{configuration["Airalo:web"]}/countries?sort=asc", cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var items = JsonConvert.DeserializeObject<List<AiraloCountryResponseView>>(json, jsonSerializerSettings) ?? [];
        return [..items.Select(x => (AiraloCountryView)x)];
    }

    public virtual async Task<List<AiraloCountryView>> GetPopularAsync(Language language, CancellationToken cancellationToken = default)
    {
        using var _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(language.ToString());

        var response = await _httpClient.GetAsync($"{configuration["Airalo:Web"]}/countries?type=popular", cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var items = JsonConvert.DeserializeObject<List<AiraloCountryResponseView>>(json, jsonSerializerSettings) ?? [];
        return [.. items.Select(x => (AiraloCountryView)x)];
    }

    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    #endregion
}
namespace myuzbekistan.Services;

public class AiraloTokenService(IConfiguration configuration) : IAiraloTokenService
{
    private readonly JsonSerializerSettings jsonSerializerSettings = new()
    {
        ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Ignore
    };

    public virtual async Task<string> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        using var _httpClient = new HttpClient();

        using var formData = new MultipartFormDataContent
        {
            { new StringContent(configuration["Airalo:ClientId"]!), "client_id" },
            { new StringContent(configuration["Airalo:ClientSecret"]!), "client_secret" },
            { new StringContent(configuration["Airalo:GrantType"]!), "grant_type" }
        };

        var response = await _httpClient.PostAsync($"{configuration["Airalo:Host"]}/v2/token", formData, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var data = JsonConvert.DeserializeObject<AiraloTokenResponseView>(json, jsonSerializerSettings);
        if (data is null || data.Data is null || string.IsNullOrEmpty(data.Data.AccessToken))
        {
            throw new BadRequestException("Failed to retrieve Airalo token.");
        }

        return data.Data.AccessToken;
    }

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
}
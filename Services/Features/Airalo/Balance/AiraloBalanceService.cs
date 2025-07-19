using Newtonsoft.Json.Serialization;

namespace myuzbekistan.Shared;

public class AiraloBalanceService(IAiraloTokenService airaloTokenService,
    IConfiguration configuration) : IAiraloBalanceService
{
    private readonly JsonSerializerSettings jsonSerializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Ignore
    };

    public virtual async Task<AiraloBalanceView> Get(CancellationToken cancellationToken = default)
    {
        await Invalidate();
        var token = await airaloTokenService.GetTokenAsync(cancellationToken);
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

        var requestUrl = $"{configuration["Airalo:Host"]}/v2/balance";

        var response = await httpClient.GetAsync(requestUrl, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonConvert.DeserializeObject<AiraloBalanceView>(content, jsonSerializerSettings)
                     ?? throw new InvalidOperationException("Failed to deserialize Airalo balance response.");

        return result;
    }

    [ComputeMethod]
    public Task<Unit> Invalidate()
    {
        Invalidation.Begin();
        return TaskExt.UnitTask;
    }
}
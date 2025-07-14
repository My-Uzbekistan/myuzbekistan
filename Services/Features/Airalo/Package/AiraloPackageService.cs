using Newtonsoft.Json.Serialization;

namespace myuzbekistan.Services;

public class AiraloPackageService(IServiceProvider services, 
    IAiraloTokenService airaloTokenService,
    IConfiguration configuration)
    : DbServiceBase<AppDbContext>(services), IAiraloPackageService
{
    private readonly JsonSerializerSettings jsonSerializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Ignore
    };

    public virtual async Task<PackageResponseView> GetCountryPackagesAsync(string countrySlug, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        var canGet = CountryCodes.Dictionary.TryGetValue(countrySlug, out var countryCode);
        if (!canGet)
        {
            Console.WriteLine(countrySlug);
            return new();
        }
        var token = await airaloTokenService.GetTokenAsync(cancellationToken);
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await httpClient.GetAsync($"{configuration["Airalo:Host"]}/v2/packages?filter[country]={countryCode}", cancellationToken);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var packages = JsonConvert.DeserializeObject<PackageResponseView>(content, jsonSerializerSettings) 
            ?? throw new InvalidOperationException("Failed to deserialize package response.");

        return packages;
    }

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    #endregion
}
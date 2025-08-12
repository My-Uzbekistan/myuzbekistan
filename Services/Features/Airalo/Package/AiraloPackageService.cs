using Newtonsoft.Json.Serialization;

namespace myuzbekistan.Services;

public class AiraloPackageService(IServiceProvider services, 
    IAiraloTokenService airaloTokenService,
    IConfiguration configuration)
    : DbServiceBase<AppDbContext>(services), IAiraloPackageService
{
    #region Queries

    private readonly JsonSerializerSettings jsonSerializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Ignore
    };

    public virtual async Task<PackageResponseView> GetCountryPackagesAsync(string? countrySlug, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        string url = $"{configuration["Airalo:Host"]}/v2/packages";
        if (!string.IsNullOrEmpty(countrySlug))
        {
            var canGet = CountryCodes.Dictionary.TryGetValue(countrySlug, out var countryCode);
            if (!canGet)
            {
                Console.WriteLine(countrySlug);
                return new();
            }
            url += $"?filter[country]={countryCode}";
        }
        var token = await airaloTokenService.GetTokenAsync(cancellationToken);
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var packages = JsonConvert.DeserializeObject<PackageResponseView>(content, jsonSerializerSettings)
            ?? throw new BadRequestException("Failed to deserialize package response.");

        return packages;
    }

    public virtual async Task<PackageResponseView> GetAllAsync(TableOptions options, string type, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        string url = $"{configuration["Airalo:Host"]}/v2/packages?filter[type]={type}&limit={options.PageSize}&page={options.Page}";
        var token = await airaloTokenService.GetTokenAsync(cancellationToken);
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var packages = JsonConvert.DeserializeObject<PackageResponseView>(content, jsonSerializerSettings)
            ?? throw new BadRequestException("Failed to deserialize package response.");

        return packages;
    }

    public virtual async Task<string> GetInstallationGuide(string iccid, Language language, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        string url = $"{configuration["Airalo:Host"]}/v2/sims/{iccid}/instructions";
        var token = await airaloTokenService.GetTokenAsync(cancellationToken);
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Add("Accept-Language", language.ToString().ToLowerInvariant());
        var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    public virtual async Task<OrderPackageStatusView> GetOrderPackageStatusAsync(string iccid, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        var token = await airaloTokenService.GetTokenAsync(cancellationToken);
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

        var requestUrl = $"{configuration["Airalo:Host"]}/v2/sims/{iccid}/usage";

        var response = await httpClient.GetAsync(requestUrl, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonConvert.DeserializeObject<OrderPackageStatusView>(content, jsonSerializerSettings)
                     ?? throw new BadRequestException("Failed to deserialize SIM usage response.");

        return result;
    }

    public virtual async Task<OrderPackageView> GetOrderPackageAsync(int id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        var token = await airaloTokenService.GetTokenAsync(cancellationToken);
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
        var requestUrl = $"{configuration["Airalo:Host"]}/v2/orders/{id}?include=sims%2Cuser%2Cstatus";
        var response = await httpClient.GetAsync(requestUrl, cancellationToken);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonConvert.DeserializeObject<OrderPackageView>(content, jsonSerializerSettings)
                     ?? throw new BadRequestException("Failed to deserialize order response.");
        return result;
    }

    #endregion

    #region Mutation

    public virtual async Task<OrderPackageView?> OrderPackageAsync(OrderArialoPackageCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            await Invalidate();
            return null;
        }

        var token = await airaloTokenService.GetTokenAsync(cancellationToken);

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var requestUrl = $"{configuration["Airalo:Host"]}/v2/orders";

        using var formContent = new MultipartFormDataContent
        {
            { new StringContent("1"), "quantity" },
            { new StringContent(command.PackageId), "package_id" },
            { new StringContent("sim"), "type" }
        };

        var response = await httpClient.PostAsync(requestUrl, formContent, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonConvert.DeserializeObject<OrderPackageView>(content, jsonSerializerSettings)
                     ?? throw new BadRequestException("Failed to deserialize order response.");

        return result;
    }

    public virtual async Task<TopupOrderView?> TopupOrderPackageAsync(TopupOrderCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            await Invalidate();
            return null;
        }

        var token = await airaloTokenService.GetTokenAsync(cancellationToken);

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var requestUrl = $"{configuration["Airalo:Host"]}/v2/orders/topups";

        using var formContent = new MultipartFormDataContent
        {
            { new StringContent(command.PackageId), "package_id" },
            { new StringContent(command.ICCID), "iccid" }
        };

        var response = await httpClient.PostAsync(requestUrl, formContent, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonConvert.DeserializeObject<TopupOrderView>(content, jsonSerializerSettings)
                     ?? throw new BadRequestException("Failed to deserialize order response.");

        return result;
    }

    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    #endregion
}
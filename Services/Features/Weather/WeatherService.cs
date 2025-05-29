using ActualLab.Async;
using ActualLab.Fusion;
using Microsoft.Extensions.Configuration;
using myuzbekistan.Shared;
using System.Net.Http.Json;
using System.Reactive;

namespace myuzbekistan.Services;

public class WeatherService(IConfiguration configuration, IRegionService regionService) : IWeatherService
{
    public async virtual Task<WeatherView> GetWeather(double lat, double lon, string lang, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        var url = configuration.GetValue<string>("WeatherApi:Url");
        var key = configuration.GetValue<string>("WeatherApi:Key");

        var client = new HttpClient();

        var response = await client.GetFromJsonAsync<WeatherResponse>($"{url}?q={lat},{lon}&key={key}&lang=ru"
            , cancellationToken: cancellationToken);
        return response == null ? throw new Exception("Weather data not found") : response.MapToView();
    }


    public async virtual Task<WeatherView> GetWeatherByRegion(long regionId, string lang, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        var url = configuration.GetValue<string>("WeatherApi:Url");
        var key = configuration.GetValue<string>("WeatherApi:Key");

        var client = new HttpClient();
        var region = await regionService.Get(regionId, cancellationToken); 
        var regionName = region.First(x=>x.Locale == "en").Name;
        if(regionName == "Republic of Karakalpakstan")
        {
            regionName = "Nukus";
        }
        if(regionName == "Uzbekistan")
        {
            regionName = "Tashkent";
        }
        var response = await client.GetFromJsonAsync<WeatherResponse>($"{url}?q={regionName}&key={key}&lang=ru"
            , cancellationToken: cancellationToken);
        return response == null ? throw new Exception("Weather data not found") : response.MapToView();
    }


    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    #endregion
}

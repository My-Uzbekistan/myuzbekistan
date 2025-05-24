using ActualLab.Async;
using ActualLab.Fusion;
using Microsoft.Extensions.Configuration;
using myuzbekistan.Shared;
using System.Net.Http.Json;
using System.Reactive;

namespace myuzbekistan.Services;

public class WeatherService(IConfiguration configuration) : IWeatherService
{
    public async virtual Task<WeatherView> GetWeather(WeatherRequest request, CancellationToken cancellationToken = default)
    {
        var url = configuration.GetValue<string>("WeatherApi:Url") ;
        var key = configuration.GetValue<string>("WeatherApi:Key") ;
        await Invalidate();
        var client = new HttpClient();
        
        var response = await client.GetFromJsonAsync<WeatherResponse>($"{url}?q={request.Lat},{request.Lon}&key={key}&lang=ru"
            ,cancellationToken: cancellationToken);
        return response == null ? throw new Exception("Weather data not found") : response.MapToView();
    }


    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    #endregion
}

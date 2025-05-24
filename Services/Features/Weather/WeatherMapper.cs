
using myuzbekistan.Shared;

namespace myuzbekistan.Services;


public static class WeatherViewExtensions
{
    public static WeatherView MapToView(this WeatherResponse response)
    {
        return new WeatherView
        {
            Temperature = $"{response.Current.TempC} °C",
            Condition = response.Current.Condition.Text,
            IconUrl = response.Current.Condition.Icon
        };
    }
}
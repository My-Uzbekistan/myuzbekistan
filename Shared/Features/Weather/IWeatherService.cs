using ActualLab.Async;
using ActualLab.Fusion;
using System.Reactive;

namespace myuzbekistan.Shared;

public interface IWeatherService : IComputeService
{
    [ComputeMethod(AutoInvalidationDelay = 60 * 60)]
    Task<WeatherView> GetWeather(double lat, double lon, string lang, CancellationToken cancellationToken = default);
    [ComputeMethod(AutoInvalidationDelay = 60 * 60)]
    Task<WeatherView> GetWeatherByRegion(long regionId, string lang, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}




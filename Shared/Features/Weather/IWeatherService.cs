using ActualLab.Async;
using ActualLab.Fusion;
using System.Reactive;

namespace myuzbekistan.Shared;

public interface IWeatherService : IComputeService
{
    [ComputeMethod]
    Task<WeatherView> GetWeather(WeatherRequest request, CancellationToken cancellationToken = default);


    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}




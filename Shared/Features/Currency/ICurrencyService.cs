using ActualLab.Async;
using ActualLab.Fusion;
using System.Reactive;

namespace myuzbekistan.Shared;
public interface ICurrencyService : IComputeService
{
    [ComputeMethod]
    Task<List<Currency>> GetCurrencies(CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<Currency> GetUsdCourse(CancellationToken cancellationToken = default);


    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}

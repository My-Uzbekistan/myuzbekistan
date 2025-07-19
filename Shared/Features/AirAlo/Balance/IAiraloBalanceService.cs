namespace myuzbekistan.Shared;


public interface IAiraloBalanceService : IComputeService
{
    [ComputeMethod]
    Task<AiraloBalanceView> Get(CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
namespace myuzbekistan.Shared;

public interface IAiraloBalanceService
{
    Task<AiraloBalanceView> Get(CancellationToken cancellationToken = default);
}
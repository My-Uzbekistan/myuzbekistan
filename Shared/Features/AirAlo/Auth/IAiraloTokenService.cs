namespace myuzbekistan.Shared;

public interface IAiraloTokenService : IComputeService
{
    [ComputeMethod(AutoInvalidationDelay = 86400)]
    Task<string> GetTokenAsync(CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
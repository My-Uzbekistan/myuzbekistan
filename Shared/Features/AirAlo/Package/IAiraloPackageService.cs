namespace myuzbekistan.Shared;

public interface IAiraloPackageService : IComputeService
{
    Task<PackageResponseView> GetCountryPackagesAsync(string countryCode, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
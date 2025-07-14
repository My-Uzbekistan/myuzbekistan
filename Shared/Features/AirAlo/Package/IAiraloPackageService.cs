namespace myuzbekistan.Shared;

public interface IAiraloPackageService : IComputeService
{
    Task<PackageResponseView> GetCountryPackagesAsync(string countrySlug, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
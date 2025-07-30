public interface IAiraloCountryService : IComputeService
{
    [ComputeMethod]
    Task<List<AiraloCountryView>> GetAllAsync(Language language, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<List<AiraloCountryView>> GetPopularAsync(Language language, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<List<AiraloCountryView>> GetRegionsAsync(Language language, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
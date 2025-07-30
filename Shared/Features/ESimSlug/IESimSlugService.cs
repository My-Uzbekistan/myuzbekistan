namespace myuzbekistan.Shared;

public interface IESimSlugService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<ESimSlugView>> GetAllCountries(TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<List<ESimSlugView>> GetAllPopularCountries(Language language, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<List<ESimSlugView>> GetAllRegions(Language language, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Sync(SyncESimSlugCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
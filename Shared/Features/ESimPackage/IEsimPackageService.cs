namespace myuzbekistan.Shared;

public interface IESimPackageService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<ESimPackageView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<UserCountsView> GetCounts(CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<ESimPackageView> Get(long Id, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<UserView> GetUserAsync(long Id, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Create(CreateESimPackageCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Update(UpdateESimPackageCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Delete(DeleteESimPackageCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task SyncPackages(SyncESimPackagesCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task UpdateDiscount(UpdatePackageDiscountCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task MakeOrder(MakeESimOrderCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
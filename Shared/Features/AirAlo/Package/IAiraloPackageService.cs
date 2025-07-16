namespace myuzbekistan.Shared;

public interface IAiraloPackageService : IComputeService
{
    [ComputeMethod]
    Task<PackageResponseView> GetCountryPackagesAsync(string countrySlug, CancellationToken cancellationToken = default);

    [ComputeMethod(AutoInvalidationDelay = 900)]
    Task<OrderPackageStatusView> GetOrderPackageStatusAsync(string iccid, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<OrderPackageView> GetOrderPackageAsync(int id, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task<OrderPackageView?> OrderPackageAsync(OrderArialoPackageCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task<TopupOrderView?> TopupOrderPackageAsync(TopupOrderCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
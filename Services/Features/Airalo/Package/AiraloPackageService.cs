namespace myuzbekistan.Services;

public class AiraloPackageService(IServiceProvider services)
    : DbServiceBase<AppDbContext>(services), IAiraloPackageService
{

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    #endregion
}
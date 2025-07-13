namespace myuzbekistan.Shared;

public interface IAiraloPackageService : IComputeService
{


    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}   
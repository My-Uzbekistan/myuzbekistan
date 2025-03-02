using ActualLab.Async;
using ActualLab.CommandR.Configuration;
using ActualLab.Fusion;
using System.Reactive;

namespace myuzbekistan.Shared;
public interface IRegionService:IComputeService
{
    [ComputeMethod]
    Task<TableResponse<RegionView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<List<RegionView>> Get(long Id, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Create(CreateRegionCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Update(UpdateRegionCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Delete(DeleteRegionCommand command, CancellationToken cancellationToken = default);
    Task<Unit> Invalidate(){ return TaskExt.UnitTask; }
    Task<List<RegionApi>> GetRegions(CancellationToken cancellationToken = default);
}
    
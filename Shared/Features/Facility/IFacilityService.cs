using ActualLab.Async;
using ActualLab.CommandR.Configuration;
using ActualLab.Fusion;
using System.Reactive;

namespace myuzbekistan.Shared;
public interface IFacilityService:IComputeService
{
    [ComputeMethod]
    Task<TableResponse<FacilityView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<List<FacilityView>> Get(long Id, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Create(CreateFacilityCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Update(UpdateFacilityCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Delete(DeleteFacilityCommand command, CancellationToken cancellationToken = default);
    Task<Unit> Invalidate(){ return TaskExt.UnitTask; }
}
    
using ActualLab.Async;
using ActualLab.CommandR.Configuration;
using ActualLab.Fusion;
using System.Reactive;

namespace myuzbekistan.Shared;
public interface IFavoriteService:IComputeService
{
    [ComputeMethod]
    Task<TableResponse<FavoriteView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<FavoriteView> Get(long Id, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Create(CreateFavoriteCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Update(UpdateFavoriteCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Delete(DeleteFavoriteCommand command, CancellationToken cancellationToken = default);
    Task<Unit> Invalidate(){ return TaskExt.UnitTask; }
}
    
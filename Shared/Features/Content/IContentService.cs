using ActualLab.Async;
using ActualLab.CommandR.Configuration;
using ActualLab.Fusion;
using System.Reactive;

namespace myuzbekistan.Shared;
public interface IContentService:IComputeService
{
    [ComputeMethod]
    Task<TableResponse<ContentView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<ContentView> Get(long Id, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Create(CreateContentCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Update(UpdateContentCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Delete(DeleteContentCommand command, CancellationToken cancellationToken = default);
    Task<Unit> Invalidate(){ return TaskExt.UnitTask; }
}
    
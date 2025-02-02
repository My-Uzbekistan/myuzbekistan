using ActualLab.Async;
using ActualLab.CommandR.Configuration;
using ActualLab.Fusion;
using System.Reactive;

namespace myuzbekistan.Shared;
public interface ITodoService:IComputeService
{
    [ComputeMethod]
    Task<TableResponse<TodoView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<TodoView> Get(long Id, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Create(CreateTodoCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Update(UpdateTodoCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Delete(DeleteTodoCommand command, CancellationToken cancellationToken = default);
    Task<Unit> Invalidate(){ return TaskExt.UnitTask; }
}
    
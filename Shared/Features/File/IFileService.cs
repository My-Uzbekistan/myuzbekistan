using ActualLab.Async;
using ActualLab.CommandR.Configuration;
using ActualLab.Fusion;
using System.Reactive;

namespace myuzbekistan.Shared;
public interface IFileService:IComputeService
{
    [ComputeMethod]
    Task<TableResponse<FileView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<FileView> Get(long Id, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Create(CreateFileCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Update(UpdateFileCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Delete(DeleteFileCommand command, CancellationToken cancellationToken = default);
    Task<Unit> Invalidate(){ return TaskExt.UnitTask; }
}
    
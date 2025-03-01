using ActualLab.Async;
using ActualLab.CommandR.Configuration;
using ActualLab.Fusion;
using System.Reactive;

namespace myuzbekistan.Shared;
public interface IContentService:IComputeService
{
    [ComputeMethod]
    Task<TableResponse<ContentView>> GetAll(long CategoryId, TableOptions options, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<List<ContentView>> Get(long Id, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Create(CreateContentCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Update(UpdateContentCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Delete(DeleteContentCommand command, CancellationToken cancellationToken = default);
    Task<Unit> Invalidate(){ return TaskExt.UnitTask; }

    Task<List<ContentApiView>> GetContents(long CategoryId, TableOptions options, CancellationToken cancellationToken = default);
    Task<ContentDto> GetContent(long ContentId, CancellationToken cancellationToken = default);
}
    
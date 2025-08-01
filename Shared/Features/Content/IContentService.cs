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

    Task<List<MainPageContent>> GetContents(long CategoryId,long userId, TableOptions options, CancellationToken cancellationToken = default, bool isNewApi = false);
    Task<ContentDto> GetContent(long ContentId,long userId, CancellationToken cancellationToken = default, bool isNewApi = false);
    Task<List<ContentShort>> GetContentByCategoryName(string CategoryName, CancellationToken cancellationToken = default, bool isNewApi = false);
    Task<List<MainPageContent>> GetContentsByIds(List<long> contentIds,long userId, TableOptions options, CancellationToken cancellationToken = default, bool isNewApi = false);

    [CommandHandler]
    Task AddRequest(AddRequestCommand command, CancellationToken cancellationToken = default);
}
    
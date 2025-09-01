using ActualLab.Async;
using ActualLab.CommandR.Configuration;
using ActualLab.Fusion;
using System.Reactive;

namespace myuzbekistan.Shared;
public interface IReviewService:IComputeService
{
    [ComputeMethod]
    Task<TableResponse<ReviewView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<TableResponse<ReviewView>> GetByContent(long contentId, TableOptions options, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<ReviewView> Get(long Id, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Create(CreateReviewCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Update(UpdateReviewCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Delete(DeleteReviewCommand command, CancellationToken cancellationToken = default);
    Task<Unit> Invalidate(){ return TaskExt.UnitTask; }
}
using ActualLab.Async;
using ActualLab.CommandR.Configuration;
using ActualLab.Fusion;
using System.Reactive;

namespace myuzbekistan.Shared;
public interface ICardService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<CardView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);

    Task<bool> CheckCard(long userId, string pan, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<List<CardInfo>> GetCardByUserId(long userId, CancellationToken cancellationToken = default);
    [ComputeMethod]
    /// Get card by userId and Id
    Task<CardView> Get(long id, long userId, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task<long> Create(CreateCardCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Update(UpdateCardCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Delete(DeleteCardCommand command, CancellationToken cancellationToken = default);
    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}

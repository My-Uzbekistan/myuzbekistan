public interface ICardPrefixService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<CardPrefixView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<CardPrefixView> Get(long Id, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Create(CreateCardPrefixCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Update(UpdateCardPrefixCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Delete(DeleteCardPrefixCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }

    Task<CardPrefixApi> GetTypeByCardNumber(string cardNumber, CancellationToken cancellationToken = default);
}
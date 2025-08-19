public interface ICardColorService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<CardColorView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<TableResponse<CardColorViewApi>> GetAllApi(TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<CardColorView> Get(long Id, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Create(CreateCardColorCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Update(UpdateCardColorCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Delete(DeleteCardColorCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
namespace myuzbekistan.Shared;

public interface IESimOrderService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<ESimOrderView>> GetAll(TableOptions options, Session? session, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<ESimOrderView> Get(long Id, Session? session, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<TableResponse<MyEsimsView>> GetAllEsim(TableOptions options, Session? session, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<EsimView> GetEsim(long Id, Session? session, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Create(CreateESimOrderCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
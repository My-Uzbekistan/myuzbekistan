namespace myuzbekistan.Shared;

public interface IESimOrderService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<ESimOrderView>> GetAll(TableOptions options, Session? session, CancellationToken cancellationToken = default);
    
    [ComputeMethod]
    Task<TableResponse<ESimOrderListView>> GetAllList(TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<ESimOrderView> Get(long Id, Session? session, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<TableResponse<MyEsimsView>> GetAllEsim(TableOptions options, Session? session, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<EsimView> GetEsim(long Id, Session? session, CancellationToken cancellationToken = default, bool exploreMore = true, long userId = 0);

    [CommandHandler]
    Task<ESimOrderView> Create(CreateESimOrderCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
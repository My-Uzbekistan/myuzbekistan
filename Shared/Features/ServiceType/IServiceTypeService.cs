public interface IServiceTypeService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<ServiceTypeView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<List<ServiceTypeView>> Get(long Id, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Create(CreateServiceTypeCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Update(UpdateServiceTypeCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Delete(DeleteServiceTypeCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
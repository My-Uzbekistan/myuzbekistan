public interface ISimCountryService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<SimCountryView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<SimCountryView> Get(long Id, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Create(CreateSimCountryCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Update(UpdateSimCountryCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Delete(DeleteSimCountryCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
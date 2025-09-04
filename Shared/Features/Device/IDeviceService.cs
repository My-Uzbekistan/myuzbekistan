public interface IDeviceService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<DeviceView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<DeviceView> Get(long Id, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Create(CreateDeviceCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Update(UpdateDeviceCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Delete(DeleteDeviceCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
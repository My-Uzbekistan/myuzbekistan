public interface IMerchantService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<MerchantView>> GetAll(long? merchantCategoryId, TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<List<MerchantView>> Get(long Id, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Create(CreateMerchantCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Update(UpdateMerchantCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Delete(DeleteMerchantCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
public interface IMerchantService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<MerchantView>> GetAll(long? merchantCategoryId, TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<TableResponse<MerchantResponse>> GetAllByApi(TableOptions options, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<MerchantResponse> GetByApi(long Id, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<List<MerchantView>> Get(long Id, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Create(CreateMerchantCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Update(UpdateMerchantCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task UpdateToken(UpdateMerchantTokenCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task AddChatId(MerchantAddChatIdCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task ClearChatId(MerchantClearChatIdCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Delete(DeleteMerchantCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
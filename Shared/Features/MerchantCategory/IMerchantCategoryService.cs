public interface IMerchantCategoryService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<MerchantCategoryView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<List<MerchantCategoryView>> Get(long Id, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Create(CreateMerchantCategoryCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Update(UpdateMerchantCategoryCommand command, CancellationToken cancellationToken = default);


    [CommandHandler]
    Task UpdateToken(UpdateMerchantCategoryTokenCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task AddChatId(MerchantCategoryAddChatIdCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task ClearChatId(MerchantCategoryClearChatIdCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Delete(DeleteMerchantCategoryCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
namespace myuzbekistan.Shared;

public interface IESimPromoCodeService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<ESimPromoCodeView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<ESimPromoCodeView> Get(long Id, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<(bool IsApplyable, string ErrorMessage)> Verify(string promoCode, long userId, long packageId, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Create(CreateESimPromoCodeCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Update(UpdateESimPromoCodeCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Delete(DeleteESimPromoCodeCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
using ActualLab.Async;
using ActualLab.CommandR.Configuration;
using ActualLab.Fusion;
using System.Reactive;

namespace myuzbekistan.Shared;
public interface IPaymentService:IComputeService
{
    [ComputeMethod]
    Task<TableResponse<PaymentView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<PaymentView> Get(long Id, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<PaymentView> GetByExternalId(string externalId, CancellationToken cancellationToken = default);
    Task<PaymentView> GetByTransactionId(string Id, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Create(CreatePaymentCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Update(UpdatePaymentCommand command, CancellationToken cancellationToken = default);
    Task ChangePaymentState(ChangePaymentStateCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Delete(DeletePaymentCommand command, CancellationToken cancellationToken = default);
    Task<Unit> Invalidate(){ return TaskExt.UnitTask; }

    
}
    
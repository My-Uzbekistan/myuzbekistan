public interface IInvoiceService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<InvoiceView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<InvoiceView> Get(long Id, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<InvoiceDetailView> GetByPaymentId(string ExternalId, CancellationToken cancellationToken = default);

    Task<TableResponse<InvoiceSummaryView>> GetByPayments(TableOptions options, long userId, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Create(CreateInvoiceCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Update(UpdateInvoiceCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task UpdateInvoiceStatus(UpdateInvoiceStatusCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Delete(DeleteInvoiceCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }

    
}
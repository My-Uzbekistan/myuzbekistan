public interface ISmsTemplateService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<SmsTemplateView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<List<SmsTemplateView>> Get(long Id, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Create(CreateSmsTemplateCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Update(UpdateSmsTemplateCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Delete(DeleteSmsTemplateCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
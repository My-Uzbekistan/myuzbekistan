using ActualLab.Async;
using ActualLab.CommandR.Configuration;
using ActualLab.Fusion;
using System.Reactive;

namespace myuzbekistan.Shared;
public interface ILanguageService:IComputeService
{
    [ComputeMethod]
    Task<TableResponse<LanguageView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<List<LanguageView>> Get(long Id, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Create(CreateLanguageCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Update(UpdateLanguageCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Delete(DeleteLanguageCommand command, CancellationToken cancellationToken = default);
    Task<Unit> Invalidate(){ return TaskExt.UnitTask; }
}
    
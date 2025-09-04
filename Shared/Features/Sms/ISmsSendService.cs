using System.Reactive;

namespace myuzbekistan.Shared;

public interface ISmsSendService : IComputeService
{
    [CommandHandler]
    Task Send(SendSmsCommand command, CancellationToken cancellationToken = default);
    Task<Unit> Invalidate() => TaskExt.UnitTask;
}

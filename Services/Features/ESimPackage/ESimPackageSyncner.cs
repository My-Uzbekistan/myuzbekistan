using Coravel.Invocable;

namespace myuzbekistan.Services;

public class ESimPackageSyncner(ICommander commander) : IInvocable
{
    public async Task Invoke()
    {
        await commander.Call(new SyncESimPackagesCommand());
    }
}
using myuzbekistan.Services;
using myuzbekistan.Shared;
using ActualLab.Fusion;

namespace myuzbekistan.Server;
public static class FusionServerExtension
{
    public static FusionBuilder AddUtcServices(this FusionBuilder fusion)
    {
        fusion.AddService<ITodoService, TodoService>();
        fusion.AddService<IAuditLogsService, AuditLogService>();
        
        return fusion;
    }
}

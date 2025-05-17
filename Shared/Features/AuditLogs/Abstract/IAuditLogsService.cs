
using ActualLab.Fusion;

namespace myuzbekistan.Shared;
public interface IAuditLogsService : IComputeService
{
    Task<TableResponse<AuditLogView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
}

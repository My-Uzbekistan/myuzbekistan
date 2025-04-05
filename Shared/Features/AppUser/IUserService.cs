using ActualLab.Async;
using ActualLab.CommandR.Configuration;
using ActualLab.Fusion;
using System.Reactive;

namespace myuzbekistan.Shared;

public interface IUserService:IComputeService
{
    [ComputeMethod]
    Task<TableResponse<ApplicationUser>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
    
    [ComputeMethod]
    Task<Unit> Invalidate() { return TaskExt.UnitTask; }

    [CommandHandler]
    Task<string> UserToExcel(UserToExcelCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task ChangeRole(ChangeRoleCommand roleCommand, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task CreateUser(CreateUserCommand roleCommand, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task ChangePassword(ChangePasswordCommand roleCommand, CancellationToken cancellationToken = default);
}

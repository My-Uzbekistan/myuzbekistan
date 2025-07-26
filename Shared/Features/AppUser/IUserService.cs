namespace myuzbekistan.Shared;

public interface IUserService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<ApplicationUser>> GetAll(TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<TableResponse<UserView>> GetAllUsers(TableOptions options, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<ApplicationUser> GetUserAsync(Session session, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<ApplicationUser> GetAsync(long id, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<Unit> Invalidate() { return TaskExt.UnitTask; }

    [CommandHandler]
    Task<string> UserToExcel(UserToExcelCommand command, CancellationToken cancellationToken = default);


    [CommandHandler]
    Task IncrementBalance(IncrementUserBalanceCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task DecrementBalance(DecrementUserBalanceCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task ChangeRole(ChangeRoleCommand roleCommand, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task CreateUser(CreateUserCommand roleCommand, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task ChangePassword(ChangePasswordCommand roleCommand, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task InvalidateUsers(InvalidateUserCommand invalidateUserCommand, CancellationToken cancellationToken = default);
}

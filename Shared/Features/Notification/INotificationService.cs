namespace myuzbekistan.Shared;

public interface INotificationService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<NotificationView>> GetAll(TableOptions options, Session session, CancellationToken cancellationToken = default);

    [ComputeMethod]
    Task<NotificationView> Get(long id, Session session, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task Create(CreateNotificationCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task MarkSeen(MarkNotificationSeenCommand command, CancellationToken cancellationToken = default);

    [CommandHandler]
    Task SetFirebaseToken(SetFirebaseTokenCommand command, CancellationToken cancellationToken = default);

    Task<Unit> Invalidate();
}

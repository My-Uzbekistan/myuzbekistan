using myuzbekistan.Services;
using myuzbekistan.Shared;
using Microsoft.EntityFrameworkCore;

public class NotificationService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), INotificationService
{
    [ComputeMethod]
    public virtual async Task<TableResponse<NotificationView>> GetAll(TableOptions options, Session session, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var db = await DbHub.CreateDbContext(cancellationToken);
        var userId = GetUserId(session);
        var query = db.Notifications.AsQueryable();
        if (!string.IsNullOrEmpty(options.Search))
            query = query.Where(x => x.Title.Contains(options.Search) || x.Content.Contains(options.Search));

        query = query.OrderByDescending(x => x.PublishAt);

        var total = await query.CountAsync(cancellationToken);
        var list = await query.Paginate(options).ToListAsync(cancellationToken);

        var seenIds = await db.NotificationReads.Where(r => r.UserId == userId && list.Select(n=>n.Id).Contains(r.NotificationId))
            .Select(r => r.NotificationId).ToListAsync(cancellationToken);

        var views = list.Select(n => n.MapToView(seenIds.Contains(n.Id))).ToList();
        return new TableResponse<NotificationView>{ Items = views, TotalItems = total };
    }

    [ComputeMethod]
    public virtual async Task<NotificationView> Get(long id, Session session, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var db = await DbHub.CreateDbContext(cancellationToken);
        var userId = GetUserId(session);
        var entity = await db.Notifications.FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? throw new MyUzException("Notification not found");
        var seen = await db.NotificationReads.AnyAsync(r => r.UserId == userId && r.NotificationId == id, cancellationToken);
        return entity.MapToView(seen);
    }

    public virtual async Task Create(CreateNotificationCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive) { _ = await Invalidate(); return; }
        await using var db = await DbHub.CreateOperationDbContext(cancellationToken);
        var entity = new NotificationEntity
        {
            Title = command.Entity.Title,
            Content = command.Entity.Content,
            Image = command.Entity.Image,
            ActionLink = command.Entity.ActionLink,
            PublishAt = command.Entity.PublishAt == default ? DateTime.UtcNow : command.Entity.PublishAt
        };
        db.Notifications.Add(entity);
        await db.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task MarkSeen(MarkNotificationSeenCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive) { _ = await Invalidate(); return; }
        await using var db = await DbHub.CreateOperationDbContext(cancellationToken);
        var userId = GetUserId(command.Session);
        var exists = await db.NotificationReads.AnyAsync(r => r.UserId == userId && r.NotificationId == command.NotificationId, cancellationToken);
        if (!exists)
        {
            db.NotificationReads.Add(new NotificationReadEntity { NotificationId = command.NotificationId, UserId = userId });
            await db.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task SetFirebaseToken(SetFirebaseTokenCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive) { _ = await Invalidate(); return; }
        await using var db = await DbHub.CreateOperationDbContext(cancellationToken);
        var userId = GetUserId(command.Session);
        var device = await db.Devices.FirstOrDefaultAsync(d => d.UserId == userId && d.FirebaseToken == command.FirebaseToken, cancellationToken);
        if (device == null)
        {
            device = new DeviceEntity
            {
                UserId = userId,
                FirebaseToken = command.FirebaseToken,
                OsVersion = command.OsVersion,
                Model = command.Model,
                AppVersion = command.AppVersion,
                Session = command.Session.Id
            };
            db.Devices.Add(device);
        }
        else
        {
            device.OsVersion = command.OsVersion;
            device.Model = command.Model;
            device.AppVersion = command.AppVersion;
            device.Session = command.Session.Id;
            db.Devices.Update(device);
        }
        await db.SaveChangesAsync(cancellationToken);
    }

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    private long GetUserId(Session session)
    {
        // naive parse from claims or session.Id placeholder; adapt later
        if (long.TryParse(session.Id, out var id)) return id;
        return 0;
    }
}

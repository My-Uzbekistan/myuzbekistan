namespace myuzbekistan.Shared;

public class NotificationEntity : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Image { get; set; }
    public string Content { get; set; } = null!;
    public string? ActionLink { get; set; }
    public DateTime PublishAt { get; set; }
    public bool IsGlobal { get; set; } = true; // future scope
}

public class NotificationReadEntity : BaseEntity
{
    public long NotificationId { get; set; }
    public long UserId { get; set; }
    public DateTime SeenAt { get; set; } = DateTime.UtcNow;
}

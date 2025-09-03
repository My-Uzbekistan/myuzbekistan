[Mapper]
public static partial class NotificationMapper
{
    public static NotificationView MapToView(this NotificationEntity src, bool isSeen) => src.To(isSeen);
    public static List<NotificationView> MapToViewList(this List<(NotificationEntity Entity,bool IsSeen)> src) => src.ToList();

    [UserMapping]
    public static partial NotificationView To(this NotificationEntity src, bool isSeen);

    private static partial List<NotificationView> ToList(this List<(NotificationEntity Entity,bool IsSeen)> src);
}

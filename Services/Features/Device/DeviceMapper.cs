[Mapper]
public static partial class DeviceMapper 
{
    #region Usable
    public static DeviceView MapToView(this DeviceEntity src) => src.To();
    public static List<DeviceView> MapToViewList(this List<DeviceEntity> src)=> src.ToList();
    public static DeviceEntity MapFromView(this DeviceView src) => src.From();
    #endregion

    #region Internal
    [UserMapping(Default = true)]
    private static partial DeviceView To(this DeviceEntity src);
    private static partial List<DeviceView> ToList(this List<DeviceEntity> src);
    private static partial DeviceEntity From(this DeviceView DeviceView);
    public static partial void From(DeviceView personView, DeviceEntity personEntity);

    #endregion
}
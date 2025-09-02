namespace myuzbekistan.Shared;

public class DeviceEntity : BaseEntity

{
    public long UserId { get; set; }

    public string FirebaseToken { get; set; } = null!;

    public string OsVersion { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string AppVersion { get; set; } = null!;

    public string Session { get; set; } = null!;

}

using System.Runtime.Serialization;
using MemoryPack;

namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
public partial record CreateNotificationCommand([property: DataMember] Session Session, [property: DataMember] NotificationView Entity) : ISessionCommand<NotificationView>;

[DataContract, MemoryPackable]
public partial record MarkNotificationSeenCommand([property: DataMember] Session Session, [property: DataMember] long NotificationId) : ISessionCommand<NotificationView>;

[DataContract, MemoryPackable]
public partial record SetFirebaseTokenCommand([property: DataMember] Session Session, [property: DataMember] string FirebaseToken, [property: DataMember] string OsVersion, [property: DataMember] string Model, [property: DataMember] string AppVersion) : ISessionCommand<DeviceView>;

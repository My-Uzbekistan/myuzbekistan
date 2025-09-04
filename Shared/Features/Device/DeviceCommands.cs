[DataContract, MemoryPackable]
public partial record CreateDeviceCommand([property: DataMember] Session Session, [property: DataMember] DeviceView Entity) : ISessionCommand<DeviceView>; 

[DataContract, MemoryPackable]
public partial record UpdateDeviceCommand([property: DataMember] Session Session, [property: DataMember] DeviceView Entity) : ISessionCommand<DeviceView>; 

[DataContract, MemoryPackable]
public partial record DeleteDeviceCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<DeviceView>; 
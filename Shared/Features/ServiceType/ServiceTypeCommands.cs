[DataContract, MemoryPackable]
public partial record CreateServiceTypeCommand([property: DataMember] Session Session, [property: DataMember] List<ServiceTypeView> Entity) : ISessionCommand<ServiceTypeView>; 

[DataContract, MemoryPackable]
public partial record UpdateServiceTypeCommand([property: DataMember] Session Session, [property: DataMember] List<ServiceTypeView> Entity) : ISessionCommand<ServiceTypeView>; 

[DataContract, MemoryPackable]
public partial record DeleteServiceTypeCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<ServiceTypeView>; 
namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
public partial record CreateESimOrderCommand([property: DataMember] Session Session, [property: DataMember] ESimOrderView Entity) : ISessionCommand<ESimOrderView>;
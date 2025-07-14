namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
public partial record CreateAiraloPackageCommand([property: DataMember] Session Session, [property: DataMember] ESimPackageView Entity) : ISessionCommand<ESimPackageView>;

[DataContract, MemoryPackable]
public partial record UpdateAiraloPackageCommand([property: DataMember] Session Session, [property: DataMember] ESimPackageView Entity) : ISessionCommand<ESimPackageView>;

[DataContract, MemoryPackable]
public partial record DeleteAiraloPackageCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<ESimPackageView>;
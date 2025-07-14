namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
public partial record CreateESimPackageCommand([property: DataMember] Session Session, [property: DataMember] ESimPackageView Entity) : ISessionCommand<ESimPackageView>;

[DataContract, MemoryPackable]
public partial record UpdateESimPackageCommand([property: DataMember] Session Session, [property: DataMember] ESimPackageView Entity) : ISessionCommand<ESimPackageView>;

[DataContract, MemoryPackable]
public partial record DeleteESimPackageCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<ESimPackageView>;

[DataContract, MemoryPackable]
public partial record SyncESimPackagesCommand() : ICommand<Unit>;
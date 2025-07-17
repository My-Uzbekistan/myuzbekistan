namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
public partial record CreateESimPackageCommand([property: DataMember] Session Session, [property: DataMember] ESimPackageView Entity) : ISessionCommand<ESimPackageView>;

[DataContract, MemoryPackable]
public partial record UpdateESimPackageCommand([property: DataMember] Session Session, [property: DataMember] ESimPackageView Entity) : ISessionCommand<ESimPackageView>;

[DataContract, MemoryPackable]
public partial record DeleteESimPackageCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<ESimPackageView>;

[DataContract, MemoryPackable]
public partial record UpdatePackageDiscountCommand([property: DataMember] Session Session, [property: DataMember] ESimPackageView Entity) : ISessionCommand<ESimPackageView>;

[DataContract, MemoryPackable]
public partial record SyncESimPackagesCommand() : ICommand<Unit>;

[DataContract, MemoryPackable]
public partial record MakeESimOrderCommand([property: DataMember] Session Session, [property: DataMember] string PackageId) : ISessionCommand<ESimOrderView>;
namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
public partial record OrderArialoPackageCommand([property: DataMember] string PackageId) : ICommand<OrderPackageView>;

[DataContract, MemoryPackable]
public partial record TopupOrderCommand([property: DataMember] string PackageId, [property: DataMember] string ICCID) : ICommand<TopupOrderView>;
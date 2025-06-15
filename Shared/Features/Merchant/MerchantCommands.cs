[DataContract, MemoryPackable]
public partial record CreateMerchantCommand([property: DataMember] Session Session, [property: DataMember] List<MerchantView> Entity) : ISessionCommand<MerchantView>; 

[DataContract, MemoryPackable]
public partial record UpdateMerchantCommand([property: DataMember] Session Session, [property: DataMember] List<MerchantView> Entity) : ISessionCommand<MerchantView>; 

[DataContract, MemoryPackable]
public partial record DeleteMerchantCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<MerchantView>; 
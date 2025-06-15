[DataContract, MemoryPackable]
public partial record CreateMerchantCategoryCommand([property: DataMember] Session Session, [property: DataMember] List<MerchantCategoryView> Entity) : ISessionCommand<MerchantCategoryView>; 

[DataContract, MemoryPackable]
public partial record UpdateMerchantCategoryCommand([property: DataMember] Session Session, [property: DataMember] List<MerchantCategoryView> Entity) : ISessionCommand<MerchantCategoryView>; 

[DataContract, MemoryPackable]
public partial record DeleteMerchantCategoryCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<MerchantCategoryView>; 
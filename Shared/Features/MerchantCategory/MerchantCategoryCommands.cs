[DataContract, MemoryPackable]
public partial record CreateMerchantCategoryCommand([property: DataMember] Session Session, [property: DataMember] List<MerchantCategoryView> Entity) : ISessionCommand<MerchantCategoryView>; 

[DataContract, MemoryPackable]
public partial record UpdateMerchantCategoryCommand([property: DataMember] Session Session, [property: DataMember] List<MerchantCategoryView> Entity) : ISessionCommand<MerchantCategoryView>;

public partial record UpdateMerchantCategoryTokenCommand([property: DataMember] Session Session, [property: DataMember] long MerchantId, [property: DataMember] string Token) : ISessionCommand<MerchantCategoryView>;


public partial record MerchantCategoryAddChatIdCommand([property: DataMember] Session Session, [property: DataMember] string Token, [property: DataMember] string ChatId) : ISessionCommand<MerchantCategoryView>;

public partial record MerchantCategoryClearChatIdCommand([property: DataMember] Session Session, [property: DataMember] long MerchantCategoryId) : ISessionCommand<MerchantCategoryView>;

[DataContract, MemoryPackable]
public partial record DeleteMerchantCategoryCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<MerchantCategoryView>;

public partial record MerchantOwnerNotifyCommand([property: DataMember] Session Session, [property: DataMember] string Token) : ISessionCommand<MerchantView>;
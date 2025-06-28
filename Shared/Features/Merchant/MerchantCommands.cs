[DataContract, MemoryPackable]
public partial record CreateMerchantCommand([property: DataMember] Session Session, [property: DataMember] List<MerchantView> Entity) : ISessionCommand<MerchantView>; 

[DataContract, MemoryPackable]
public partial record UpdateMerchantCommand([property: DataMember] Session Session, [property: DataMember] List<MerchantView> Entity) : ISessionCommand<MerchantView>;


public partial record UpdateMerchantTokenCommand([property: DataMember] Session Session, [property: DataMember] long MerchantId, [property: DataMember] string Token) : ISessionCommand<MerchantView>;


public partial record MerchantAddChatIdCommand([property: DataMember] Session Session, [property: DataMember] string Token, [property: DataMember] string ChatId) : ISessionCommand<MerchantView>;

public partial record MerchantClearChatIdCommand([property: DataMember] Session Session, [property: DataMember] long MerchantId) : ISessionCommand<MerchantView>;

[DataContract, MemoryPackable]
public partial record DeleteMerchantCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<MerchantView>; 
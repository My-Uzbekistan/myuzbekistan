[DataContract, MemoryPackable]
public partial record CreateCardPrefixCommand([property: DataMember] Session Session, [property: DataMember] CardPrefixView Entity) : ISessionCommand<CardPrefixView>; 

[DataContract, MemoryPackable]
public partial record UpdateCardPrefixCommand([property: DataMember] Session Session, [property: DataMember] CardPrefixView Entity) : ISessionCommand<CardPrefixView>; 

[DataContract, MemoryPackable]
public partial record DeleteCardPrefixCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<CardPrefixView>; 
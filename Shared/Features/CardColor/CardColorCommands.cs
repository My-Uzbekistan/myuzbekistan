[DataContract, MemoryPackable]
public partial record CreateCardColorCommand([property: DataMember] Session Session, [property: DataMember] CardColorView Entity) : ISessionCommand<CardColorView>; 

[DataContract, MemoryPackable]
public partial record UpdateCardColorCommand([property: DataMember] Session Session, [property: DataMember] CardColorView Entity) : ISessionCommand<CardColorView>; 

[DataContract, MemoryPackable]
public partial record DeleteCardColorCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<CardColorView>; 
namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
public partial record CreateESimPromoCodeCommand([property: DataMember] Session Session, [property: DataMember] ESimPromoCodeView Entity) : ISessionCommand<ESimPromoCodeView>;

[DataContract, MemoryPackable]
public partial record UpdateESimPromoCodeCommand([property: DataMember] Session Session, [property: DataMember] ESimPromoCodeView Entity) : ISessionCommand<ESimPromoCodeView>;

[DataContract, MemoryPackable]
public partial record DeleteESimPromoCodeCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<ESimPromoCodeView>;
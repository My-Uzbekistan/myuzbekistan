[DataContract, MemoryPackable]
public partial record CreateSimCountryCommand([property: DataMember] Session Session, [property: DataMember] SimCountryView Entity) : ISessionCommand<SimCountryView>; 

[DataContract, MemoryPackable]
public partial record UpdateSimCountryCommand([property: DataMember] Session Session, [property: DataMember] SimCountryView Entity) : ISessionCommand<SimCountryView>; 

[DataContract, MemoryPackable]
public partial record DeleteSimCountryCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<SimCountryView>; 
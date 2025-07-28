using MemoryPack;
using ActualLab.CommandR;
using ActualLab.Fusion;
using System.Runtime.Serialization;


namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
public partial record CreateCardCommand([property: DataMember] Session Session,[property: DataMember] CardView Entity):ISessionCommand<long>; 

[DataContract, MemoryPackable]
public partial record UpdateCardCommand([property: DataMember] Session Session,[property: DataMember] CardView Entity):ISessionCommand<CardView>; 

[DataContract, MemoryPackable]
public partial record DeleteCardCommand([property: DataMember] Session Session,[property: DataMember] long Id,[property: DataMember] long UserId):ISessionCommand<CardView>; 


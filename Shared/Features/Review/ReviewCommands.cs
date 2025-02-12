using MemoryPack;
using ActualLab.CommandR;
using ActualLab.Fusion;
using System.Runtime.Serialization;


namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
public partial record CreateReviewCommand([property: DataMember] Session Session,[property: DataMember] ReviewView Entity):ISessionCommand<ReviewView>; 

[DataContract, MemoryPackable]
public partial record UpdateReviewCommand([property: DataMember] Session Session,[property: DataMember] ReviewView Entity):ISessionCommand<ReviewView>; 

[DataContract, MemoryPackable]
public partial record DeleteReviewCommand([property: DataMember] Session Session,[property: DataMember] long Id):ISessionCommand<ReviewView>; 


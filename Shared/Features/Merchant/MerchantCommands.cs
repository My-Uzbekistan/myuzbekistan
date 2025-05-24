using ActualLab.Fusion;
using MemoryPack;
using System.Runtime.Serialization;

[DataContract, MemoryPackable]
public partial record CreateMerchantCommand([property: DataMember] Session Session, [property: DataMember] MerchantView Entity) : ISessionCommand<MerchantView>; 

[DataContract, MemoryPackable]
public partial record UpdateMerchantCommand([property: DataMember] Session Session, [property: DataMember] MerchantView Entity) : ISessionCommand<MerchantView>; 

[DataContract, MemoryPackable]
public partial record DeleteMerchantCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<MerchantView>; 
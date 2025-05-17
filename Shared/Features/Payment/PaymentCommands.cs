using MemoryPack;
using ActualLab.CommandR;
using ActualLab.Fusion;
using System.Runtime.Serialization;


namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
public partial record CreatePaymentCommand([property: DataMember] Session Session,[property: DataMember] PaymentView Entity):ISessionCommand<PaymentView>; 

[DataContract, MemoryPackable]
public partial record UpdatePaymentCommand([property: DataMember] Session Session,[property: DataMember] PaymentView Entity):ISessionCommand<PaymentView>; 

[DataContract, MemoryPackable]
public partial record DeletePaymentCommand([property: DataMember] Session Session,[property: DataMember] long Id):ISessionCommand<PaymentView>; 


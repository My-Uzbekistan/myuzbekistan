using MemoryPack;
using ActualLab.CommandR;
using ActualLab.Fusion;
using System.Runtime.Serialization;


namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
public partial record CreateFacilityCommand([property: DataMember] Session Session,[property: DataMember] List<FacilityView> Entity):ISessionCommand<FacilityView>; 

[DataContract, MemoryPackable]
public partial record UpdateFacilityCommand([property: DataMember] Session Session,[property: DataMember] List<FacilityView> Entity):ISessionCommand<FacilityView>; 

[DataContract, MemoryPackable]
public partial record DeleteFacilityCommand([property: DataMember] Session Session,[property: DataMember] long Id):ISessionCommand<FacilityView>; 


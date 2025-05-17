using MemoryPack;
using ActualLab.CommandR;
using ActualLab.Fusion;
using System.Runtime.Serialization;


namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
public partial record CreateRegionCommand([property: DataMember] Session Session,[property: DataMember] List<RegionView> Entity):ISessionCommand<RegionView>; 

[DataContract, MemoryPackable]
public partial record UpdateRegionCommand([property: DataMember] Session Session,[property: DataMember] List<RegionView> Entity):ISessionCommand<RegionView>; 

[DataContract, MemoryPackable]
public partial record DeleteRegionCommand([property: DataMember] Session Session,[property: DataMember] long Id):ISessionCommand<RegionView>; 


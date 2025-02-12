using MemoryPack;
using ActualLab.CommandR;
using ActualLab.Fusion;
using System.Runtime.Serialization;


namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
public partial record CreateContentCommand([property: DataMember] Session Session,[property: DataMember] ContentView Entity):ISessionCommand<ContentView>; 

[DataContract, MemoryPackable]
public partial record UpdateContentCommand([property: DataMember] Session Session,[property: DataMember] ContentView Entity):ISessionCommand<ContentView>; 

[DataContract, MemoryPackable]
public partial record DeleteContentCommand([property: DataMember] Session Session,[property: DataMember] long Id):ISessionCommand<ContentView>; 


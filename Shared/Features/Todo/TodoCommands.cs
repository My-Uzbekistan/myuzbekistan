using MemoryPack;
using ActualLab.CommandR;
using ActualLab.Fusion;
using System.Runtime.Serialization;


namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
public partial record CreateTodoCommand([property: DataMember] Session Session,[property: DataMember] TodoView Entity):ISessionCommand<TodoView>; 

[DataContract, MemoryPackable]
public partial record UpdateTodoCommand([property: DataMember] Session Session,[property: DataMember] TodoView Entity):ISessionCommand<TodoView>; 

[DataContract, MemoryPackable]
public partial record DeleteTodoCommand([property: DataMember] Session Session,[property: DataMember] long Id):ISessionCommand<TodoView>; 


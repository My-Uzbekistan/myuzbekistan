using MemoryPack;
using ActualLab.CommandR;
using ActualLab.Fusion;
using System.Runtime.Serialization;


namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
public partial record CreateCategoryCommand([property: DataMember] Session Session,[property: DataMember] List<CategoryView> Entity):ISessionCommand<CategoryView>; 

[DataContract, MemoryPackable]
public partial record UpdateCategoryCommand([property: DataMember] Session Session,[property: DataMember] List<CategoryView> Entity) :ISessionCommand<CategoryView>; 

[DataContract, MemoryPackable]
public partial record DeleteCategoryCommand([property: DataMember] Session Session,[property: DataMember] long Id):ISessionCommand<CategoryView>; 


using MemoryPack;
using ActualLab.CommandR;
using ActualLab.Fusion;
using System.Runtime.Serialization;


namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
public partial record CreateFileCommand([property: DataMember] Session Session,[property: DataMember] FileView Entity):ISessionCommand<FileView>; 

[DataContract, MemoryPackable]
public partial record UpdateFileCommand([property: DataMember] Session Session,[property: DataMember] FileView Entity):ISessionCommand<FileView>; 

[DataContract, MemoryPackable]
public partial record DeleteFileCommand([property: DataMember] Session Session,[property: DataMember] long Id):ISessionCommand<FileView>; 


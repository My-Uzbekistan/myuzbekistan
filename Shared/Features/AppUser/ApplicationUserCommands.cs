using MemoryPack;
using ActualLab.CommandR;
using ActualLab.Fusion;
using System.Runtime.Serialization;


namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
public partial record CreateApplicationUserCommand([property: DataMember] Session Session,[property: DataMember] List<ApplicationUser> Entity):ISessionCommand<ApplicationUser>; 

[DataContract, MemoryPackable]
public partial record UpdateApplicationUserCommand([property: DataMember] Session Session,[property: DataMember] List<ApplicationUser> Entity) :ISessionCommand<ApplicationUser>; 

[DataContract, MemoryPackable]
public partial record DeleteApplicationUserCommand([property: DataMember] Session Session,[property: DataMember] long Id):ISessionCommand<ApplicationUser>;

[DataContract, MemoryPackable]
public partial record UserToExcelCommand(
    [property: DataMember] Session Session,
    [property: DataMember] TableOptions Options) : ISessionCommand<string>;

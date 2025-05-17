using MemoryPack;
using ActualLab.CommandR;
using ActualLab.Fusion;
using System.Runtime.Serialization;


namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
public partial record CreateUserCommand([property: DataMember] Session Session, [property: DataMember] CreateUser User) : ISessionCommand<ApplicationUser>;


[DataContract, MemoryPackable]
public partial record IncrementUserBalanceCommand([property: DataMember] Session Session, [property: DataMember] long UserId, [property: DataMember] decimal Balance) : ISessionCommand<ApplicationUser>;

[DataContract, MemoryPackable]
public partial record DecrementUserBalanceCommand([property: DataMember] Session Session, [property: DataMember] long UserId, [property: DataMember] decimal Balance) : ISessionCommand<ApplicationUser>;

[DataContract, MemoryPackable]
public partial record ChangePasswordCommand([property: DataMember] Session Session, [property: DataMember] long UserId, [property: DataMember] string Password) : ISessionCommand<ApplicationUser>;

[DataContract, MemoryPackable]
public partial record InvalidateUserCommand([property: DataMember] Session Session) : ISessionCommand<ApplicationUser>;


[DataContract, MemoryPackable]
public partial record UserToExcelCommand(
    [property: DataMember] Session Session,
    [property: DataMember] TableOptions Options) : ISessionCommand<string>;

[DataContract, MemoryPackable]
public partial record ChangeRoleCommand(
    [property: DataMember] Session Session,
    [property: DataMember] long UserId,
    [property: DataMember] string Role) : ISessionCommand<string>;


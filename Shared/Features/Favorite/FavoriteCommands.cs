using MemoryPack;
using ActualLab.CommandR;
using ActualLab.Fusion;
using System.Runtime.Serialization;


namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
public partial record CreateFavoriteCommand([property: DataMember] Session Session, [property: DataMember]  long ContentId, [property: DataMember] long UserId) : ISessionCommand<FavoriteView>;

[DataContract, MemoryPackable]
public partial record UpdateFavoriteCommand([property: DataMember] Session Session, [property: DataMember] FavoriteView Entity) : ISessionCommand<FavoriteView>;

[DataContract, MemoryPackable]
public partial record DeleteFavoriteCommand([property: DataMember] Session Session, [property: DataMember] long ContentId, [property: DataMember] long UserId) : ISessionCommand<FavoriteView>;


[DataContract, MemoryPackable]
public partial record FavoriteToExcelCommand(
    [property: DataMember] Session Session,
    [property: DataMember] TableOptions Options) : ISessionCommand<string>;



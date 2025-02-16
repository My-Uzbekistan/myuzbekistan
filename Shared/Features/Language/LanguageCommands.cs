using MemoryPack;
using ActualLab.CommandR;
using ActualLab.Fusion;
using System.Runtime.Serialization;


namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
public partial record CreateLanguageCommand([property: DataMember] Session Session,[property: DataMember] List<LanguageView> Entity):ISessionCommand<LanguageView>; 

[DataContract, MemoryPackable]
public partial record UpdateLanguageCommand([property: DataMember] Session Session,[property: DataMember] List<LanguageView> Entity):ISessionCommand<LanguageView>; 

[DataContract, MemoryPackable]
public partial record DeleteLanguageCommand([property: DataMember] Session Session,[property: DataMember] long Id):ISessionCommand<LanguageView>; 


[DataContract, MemoryPackable]
public partial record CreateSmsTemplateCommand([property: DataMember] Session Session, [property: DataMember] List<SmsTemplateView> Entity) : ISessionCommand<SmsTemplateView>; 

[DataContract, MemoryPackable]
public partial record UpdateSmsTemplateCommand([property: DataMember] Session Session, [property: DataMember] List<SmsTemplateView> Entity) : ISessionCommand<SmsTemplateView>; 

[DataContract, MemoryPackable]
public partial record DeleteSmsTemplateCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<SmsTemplateView>; 
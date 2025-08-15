[DataContract, MemoryPackable]
public partial record CreateInvoiceCommand([property: DataMember] Session Session, [property: DataMember] InvoiceRequest InvoiceRequest) : ISessionCommand<InvoiceView>; 

[DataContract, MemoryPackable]
public partial record UpdateInvoiceCommand([property: DataMember] Session Session, [property: DataMember] InvoiceView Entity) : ISessionCommand<InvoiceView>;

[DataContract, MemoryPackable]
public partial record UpdateInvoiceStatusCommand([property: DataMember] Session Session, [property: DataMember] PaymentStatus Status, string ExternalId) : ISessionCommand<InvoiceView>;


[DataContract, MemoryPackable]
public partial record DeleteInvoiceCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<InvoiceView>; 
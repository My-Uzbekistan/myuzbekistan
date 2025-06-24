namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class InvoiceRequest
{
    [property: DataMember] public Decimal Amount { get; set; }
    [property: DataMember] public string? Description { get; set; }
    [property: DataMember] public long MerchantId { get; set; }
    
}
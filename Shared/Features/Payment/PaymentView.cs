using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.Runtime.Serialization;

namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PaymentView
{
   [property: DataMember] public long UserId { get; set; }
   [property: DataMember] public string? PaymentMethod { get; set; } 
   [property: DataMember] public Decimal Amount { get; set; }
   [property: DataMember] public PaymentStatus PaymentStatus { get; set; } 
   [property: DataMember] public string? TransactionId { get; set; } 
   [property: DataMember] public string? CallbackData { get; set; } 
   [property: DataMember] public long Id { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as PaymentView;
        return other?.Id == Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
}

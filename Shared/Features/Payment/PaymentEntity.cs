using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace myuzbekistan.Shared;


[Index(nameof(UserId))]
[Index(nameof(CardId))]
[SkipGeneration]
public partial class PaymentEntity : BaseEntity
{
    
    public long UserId { get; set; }
    public long CardId { get; set; }
    public string? PaymentMethod { get; set; } = "multicard";
    public decimal Amount { get; set; }
    public decimal UserBalance { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public string? TransactionId { get; set; } = null;
    public string? ExternalId { get; set; } = null;
    [Column(TypeName = "jsonb")]
    public string? CallbackData { get; set; }


}


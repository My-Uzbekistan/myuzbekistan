using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace myuzbekistan.Shared;  

[Index(nameof(UserId))]
[SkipGeneration]
public partial class CardEntity : BaseEntity
{
    public string? Code { get; set; } = null!;
    public string? Name { get; set; } = null!;

    public string? Cvv { get; set; } = null!;

    public long? CardId { get; set; }
    public long UserId { get; set; }
    public string? ExpirationDate { get; set; } // MM/YY
    public int? ApplicationId { get; set; }
    public string? PayerId { get; set; } = null!;
    public string? CardPan { get; set; } = null!;
    public string? CardToken { get; set; } = null!;
    public string? Phone { get; set; } = null!;
    public string? HolderName { get; set; } = null!;
    public string? Pinfl { get; set; }
    public string? Ps { get; set; } = null!;
    public string? Status { get; set; }
    public string? AddedOn { get; set; }
    [Column(TypeName = "jsonb")]
    public CardStatus? CardStatus { get; set; }
    public bool? SmsInform { get; set; }
    public bool? IsMulticard { get; set; }
}

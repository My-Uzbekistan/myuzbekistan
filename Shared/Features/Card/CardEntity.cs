using Microsoft.EntityFrameworkCore;

namespace myuzbekistan.Shared;  

[Index(nameof(UserId))]
[SkipGeneration]
public partial class CardEntity : BaseEntity
{
    public CardColorEntity Code { get; set; } = null!;
    public string? Name { get; set; } = null!;
    public long? CardId { get; set; }
    public long UserId { get; set; }
    public string? ExpirationDate { get; set; } // MM/YY
    public string? CardPan { get; set; } = null!;
    public string? CardToken { get; set; } = null!;
    public string? Phone { get; set; } = null!;
    public string? HolderName { get; set; } = null!;
    public string? Pinfl { get; set; }
    public string? Ps { get; set; } = null!;
    public string? Status { get; set; }
    
}

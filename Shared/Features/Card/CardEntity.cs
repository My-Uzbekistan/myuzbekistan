using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Text.Json.Serialization;

namespace myuzbekistan.Shared;  

[Index(nameof(UserId))]
[SkipGeneration]
public partial class CardEntity : BaseEntity
{
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
    public int? Cvv { get; set; } 
    public bool IsExternal
    {
        get => Ps == "VISA" || Ps == "MASTERCARD";
        set { /* Needed for serialization, but can be left empty or throw if you want to make it read-only */ }
    }
    public string? Icon { get; set; } = null!;

}

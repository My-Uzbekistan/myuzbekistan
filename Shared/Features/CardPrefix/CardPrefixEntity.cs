using Microsoft.EntityFrameworkCore;

namespace myuzbekistan.Shared;

public class CardPrefixEntity : BaseEntity
{
    public uint Prefix { get; set; }
    public string BankName { get; set; } = null!;
    public string CardType { get; set; } = null!;
    public FileEntity? CardBrand { get; set; } = null!;
}

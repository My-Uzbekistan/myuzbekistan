using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myuzbekistan.Shared;

public class CardPrefixEntity : BaseEntity
{
    public string? Prefix { get; set; } = null!;
    public string? BankName { get; set; } = null!;
    public string? CardType { get; set; } = null!;
    public string? CardBrand { get; set; } = null!;
    public string? CardProduct { get; set; } = null!;
    public string? CardIssuer { get; set; } = null!;
}

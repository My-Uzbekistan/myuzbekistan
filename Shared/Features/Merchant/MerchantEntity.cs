using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myuzbekistan.Shared;

public class MerchantEntity : BaseEntity
{
    public string? Name { get; set; } = null!;
    public string? Phone { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public string? Address { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public string? Image { get; set; } = null!;
    public bool IsActive { get; set; } = false;

    public MerchantEntity? Parent { get; set; }
}

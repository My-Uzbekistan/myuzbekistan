﻿using System;
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
    public string? Contract { get; set; } = null!;
    public string Inn { get; set; } = null!;
    public string? Mfi { get; set; } = null!;
    public string CurrentAccount { get; set; } = null!;
    public short Discount { get; set; } = 0;
    public bool IsVat { get; set; }
    public FileEntity? Image { get; set; } = null!;
    public bool IsActive { get; set; } = false;
    public MerchantEntity? Parent { get; set; }
}

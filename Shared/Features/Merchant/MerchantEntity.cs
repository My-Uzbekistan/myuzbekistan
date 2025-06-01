using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myuzbekistan.Shared;

public class MerchantEntity : BaseEntity
{
    public FileEntity? Logo { get; set; } = null!;
    public string? Name { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public string? Address { get; set; } = null!;
    public string? MXIK { get; set; } = null!;
    public string? WorkTime { get; set; } = null!;
    public string? Phone { get; set; } = null!;
    public string Responsible { get; set; } = null!;
    public bool Status { get; set; } = false;
    public MerchantCategoryEntity MerchantCategory { get; set; } = null!;
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = NetTopologySuite.Geometries.Point;

namespace myuzbekistan.Shared;

[PrimaryKey(nameof(MerchantEntity.Id), nameof(MerchantEntity.Locale))]
public class MerchantEntity : BaseEntity
{
    public string Locale { get; set; } = null!;
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
    public Point? Location { get; set; }
}

using Microsoft.EntityFrameworkCore;
using myuzbekistan.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myuzbekistan.Shared;

[PrimaryKey(nameof(CategoryEntity.Id), nameof(CategoryEntity.Locale))]
[SkipGeneration]
public class RegionEntity:BaseEntity
{
    public string Name { get; set; }
    public string Locale { get; set; }
    public bool IsActive { get; set; } 
    public RegionEntity? ParentRegion { get; set; }
    public long? ParentRegionId { get; set; }
    public List<ContentEntity>? Contents { get; set; }
}

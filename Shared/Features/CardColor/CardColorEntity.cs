using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myuzbekistan.Shared;

public class CardColorEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public string ColorCode { get; set; } = null!;
}

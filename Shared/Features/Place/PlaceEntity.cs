using myuzbekistan.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace myuzbekistan.Shared;
class PlaceEntity : BaseEntity
{
    [Required]
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    [Column(TypeName = "geometry")] public Point? Location { get; set; }
    public double Rating { get; set; }

    public ICollection<FileEntity>? Images { get; set; }
}


public class RestaurantEntity : BaseEntity
{

    [Required]
    public string Name { get; set; } = null!;
    public string Cuisine { get; set; } = null!;
    [Column(TypeName = "geometry")] public Point? Location { get; set; }
    [Required]
    public double Rating { get; set; }
    public string PriceRange { get; set; } = null!;  // $$$, $$, etc.

    public ICollection<FileEntity>? Images { get; set; }

    public FileEntity? Menu { get; set; }
}
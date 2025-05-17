using BlazorLeaflet.Models;

namespace Client.Core.Components.MapLeafLet;

public class LeafLetMarker
{
    public string? Subdivision { get; set; }
    public string? Place { get; set; }
    public string? CarNumber { get; set; }
    public string? Name { get; set; }
    public string? Role { get; set; }
    public string? Status { get; set; }
    public float Lat { get; set; }
    public float Lng { get; set; }
    public bool? EngineOn { get; set; } = null;
    public int? Number { get; set; } = null;
}
public class Location
{
    public string City { get; set; }
    public string Address { get; set; }
    public LatLng LatLng { get; set; }
    public int Number { get; set; }
}

public class LatLngDouble
{
    public double Lng { get; set; }
    public double Lat { get; set; }
}

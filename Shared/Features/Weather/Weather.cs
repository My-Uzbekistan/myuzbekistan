using ActualLab.Fusion.Blazor;
using MemoryPack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace myuzbekistan.Shared;

public class WeatherCondition
{
    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("icon")]
    public string Icon { get; set; }

    [JsonPropertyName("code")]
    public int Code { get; set; }
}

public class WeatherCurrent
{
    [JsonPropertyName("last_updated_epoch")]
    public int LastUpdatedEpoch { get; set; }

    [JsonPropertyName("last_updated")]
    public string LastUpdated { get; set; } = null!;

    [JsonPropertyName("temp_c")]
    public double TempC { get; set; }

    [JsonPropertyName("temp_f")]
    public double TempF { get; set; }

    [JsonPropertyName("is_day")]
    public int IsDay { get; set; }

    [JsonPropertyName("condition")]
    public WeatherCondition Condition { get; set; } = null!;

    [JsonPropertyName("wind_mph")]
    public double WindMph { get; set; }

    [JsonPropertyName("wind_kph")]
    public double WindKph { get; set; }

    [JsonPropertyName("wind_degree")]
    public int WindDegree { get; set; }

    [JsonPropertyName("wind_dir")]
    public string WindDir { get; set; }

    [JsonPropertyName("pressure_mb")]
    public double PressureMb { get; set; }

    [JsonPropertyName("pressure_in")]
    public double PressureIn { get; set; }

    [JsonPropertyName("precip_mm")]
    public double PrecipMm { get; set; }

    [JsonPropertyName("precip_in")]
    public double PrecipIn { get; set; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }

    [JsonPropertyName("cloud")]
    public int Cloud { get; set; }

    [JsonPropertyName("feelslike_c")]
    public double FeelslikeC { get; set; }

    [JsonPropertyName("feelslike_f")]
    public double FeelslikeF { get; set; }

    [JsonPropertyName("windchill_c")]
    public double WindchillC { get; set; }

    [JsonPropertyName("windchill_f")]
    public double WindchillF { get; set; }

    [JsonPropertyName("heatindex_c")]
    public double HeatndexC { get; set; }

    [JsonPropertyName("heatindex_f")]
    public double HeatindexF { get; set; }

    [JsonPropertyName("dewpoint_c")]
    public double DewpointC { get; set; }

    [JsonPropertyName("dewpoint_f")]
    public double DewpointF { get; set; }

    [JsonPropertyName("vis_km")]
    public double VisKm { get; set; }

    [JsonPropertyName("vis_miles")]
    public double VisMiles { get; set; }

    [JsonPropertyName("uv")]
    public double Uv { get; set; }

    [JsonPropertyName("gust_mph")]
    public double GustMph { get; set; }

    [JsonPropertyName("gust_kph")]
    public double GustKph { get; set; }
}

public class WeatherLocation
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lon")]
    public double Lon { get; set; }

    [JsonPropertyName("tz_id")]
    public string TzId { get; set; }

    [JsonPropertyName("localtime_epoch")]
    public int LocaltimeEpoch { get; set; }

    [JsonPropertyName("localtime")]
    public string Localtime { get; set; }
}

public class WeatherResponse
{
    [JsonPropertyName("location")]
    public WeatherLocation Location { get; set; }

    [JsonPropertyName("current")]
    public WeatherCurrent Current { get; set; }
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class WeatherView
{
    [property: DataMember]
    public string Temperature { get; set; } = null!;
    [property: DataMember]
    public string Condition { get; set; } = null!;
    [property: DataMember]
    public string IconUrl { get; set; } = null!;
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class WeatherRequest
{
    [Required]
    [JsonPropertyName("lat")]
    [property: DataMember]
    public double Lat { get; set; }
    [JsonPropertyName("lon")]
    [Required]
    [property: DataMember]
    public double Lon { get; set; }

    [property: DataMember]
    [JsonPropertyName("lang")]
    public string? Lang { get; set; } 
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class WeatherByRegionRequest
{
    [Required]
    [JsonPropertyName("regionId")]
    [property: DataMember]
    public long RegionId { get; set; }

    [property: DataMember]
    [JsonPropertyName("lang")]
    public string? Lang { get; set; }
}








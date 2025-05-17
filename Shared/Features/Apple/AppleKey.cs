using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myuzbekistan.Shared;

public class AppleKey
{
    [JsonProperty("kty")]
    public string Kty { get; set; } = default!;

    [JsonProperty("kid")]
    public string Kid { get; set; } = default!;

    [JsonProperty("use")]
    public string Use { get; set; } = default!;

    [JsonProperty("alg")]
    public string Alg { get; set; } = default!;

    [JsonProperty("n")]
    public string N { get; set; } = default!;

    [JsonProperty("e")]
    public string E { get; set; } = default!;
}
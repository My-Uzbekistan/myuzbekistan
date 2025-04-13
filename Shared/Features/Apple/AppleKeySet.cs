using Newtonsoft.Json;

namespace myuzbekistan.Shared;

public class AppleKeySet
{
    [JsonProperty("keys")]
    public List<AppleKey> Keys { get; set; } = new();
}

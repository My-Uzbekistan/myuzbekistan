namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class AiraloTokenResponseView
{
    [DataMember, JsonProperty("data")]
    public AiraloTokenResponseData Data { get; set; } = new();
    [DataMember, JsonProperty("meta")]
    public AiraloTokenResponseMeta Meta { get; set; } = new();
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class AiraloTokenResponseData
{
    [DataMember, JsonProperty("token_type")]
    public string TokenType { get; set; } = string.Empty;
    [DataMember, JsonProperty("expires_in")]
    public int ExpiresIn { get; set; } = 0;
    [DataMember, JsonProperty("access_token")]
    public string AccessToken { get; set; } = string.Empty;
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class AiraloTokenResponseMeta
{
    [DataMember, JsonProperty("message")]
    public string? Message { get; set; }
}

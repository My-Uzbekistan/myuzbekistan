namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class OrderPackageStatusView
{
    [DataMember, JsonProperty("data")]
    public OrderPackageStatusData Data { get; set; } = new();

    [DataMember, JsonProperty("meta")]
    public OrderPackageStatusMeta Meta { get; set; } = new();
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class OrderPackageStatusData
{
    [DataMember, JsonProperty("remaining")]
    public int Remaining { get; set; }

    [DataMember, JsonProperty("total")]
    public int Total { get; set; }

    [DataMember, JsonProperty("expired_at")]
    public string ExpiredAt { get; set; } = string.Empty;

    [DataMember, JsonProperty("is_unlimited")]
    public bool IsUnlimited { get; set; }

    [DataMember, JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    [DataMember, JsonProperty("remaining_voice")]
    public int RemainingVoice { get; set; }

    [DataMember, JsonProperty("remaining_text")]
    public int RemainingText { get; set; }

    [DataMember, JsonProperty("total_voice")]
    public int TotalVoice { get; set; }

    [DataMember, JsonProperty("total_text")]
    public int TotalText { get; set; }
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class OrderPackageStatusMeta
{
    [DataMember, JsonProperty("message")]
    public string Message { get; set; } = string.Empty;
}
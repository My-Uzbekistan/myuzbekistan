namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class AiraloBalanceView
{
    [DataMember, JsonProperty("data")]
    public AiraloBalanceData Data { get; set; } = new();
    [DataMember, JsonProperty("meta")]
    public AiraloBalanceMeta Meta { get; set; } = new();
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class AiraloBalanceData
{
    [DataMember, JsonProperty("balances")]
    public AiraloBalanceBalances Balances { get; set; } = new();
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class AiraloBalanceBalances
{
    [DataMember, JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    [DataMember, JsonProperty("availableBalance")]
    public AiraloBalanceAvailablebalance AvailableBalance { get; set; } = new();
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class AiraloBalanceAvailablebalance
{
    [DataMember, JsonProperty("amount")]
    public double Amount { get; set; }
    [DataMember, JsonProperty("currency")]
    public string Aurrency { get; set; } = string.Empty;
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class AiraloBalanceMeta
{
    [DataMember, JsonProperty("message")]
    public string Message { get; set; } = string.Empty;
}
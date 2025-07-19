namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class TopupOrderView
{
    [DataMember, JsonProperty("data")]
    public TopupOrderData Data { get; set; } = new();

    [DataMember, JsonProperty("meta")]
    public TopupOrderMeta Meta { get; set; } = new();
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class TopupOrderData
{
    [DataMember, JsonProperty("package_id")]
    public string PackageId { get; set; } = string.Empty;

    [DataMember, JsonProperty("quantity")]
    public string Quantity { get; set; } = string.Empty;

    [DataMember, JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [DataMember, JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    [DataMember, JsonProperty("esim_type")]
    public string EsimType { get; set; } = string.Empty;

    [DataMember, JsonProperty("validity")]
    public int Validity { get; set; }

    [DataMember, JsonProperty("package")]
    public string Package { get; set; } = string.Empty;

    [DataMember, JsonProperty("data")]
    public string Data { get; set; } = string.Empty;

    [DataMember, JsonProperty("price")]
    public int Price { get; set; }

    [DataMember, JsonProperty("created_at")]
    public string CreatedAt { get; set; } = string.Empty;

    [DataMember, JsonProperty("id")]
    public int Id { get; set; }

    [DataMember, JsonProperty("code")]
    public string Code { get; set; } = string.Empty;

    [DataMember, JsonProperty("currency")]
    public string Currency { get; set; } = string.Empty;

    [DataMember, JsonProperty("manual_installation")]
    public string ManualInstallation { get; set; } = string.Empty;

    [DataMember, JsonProperty("qrcode_installation")]
    public string QrcodeInstallation { get; set; } = string.Empty;

    [DataMember, JsonProperty("installation_guides")]
    public TopupOrderInstallationGuides InstallationGuides { get; set; } = new();
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class TopupOrderInstallationGuides
{
    [DataMember, JsonProperty("en")]
    public string En { get; set; } = string.Empty;
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class TopupOrderMeta
{
    [DataMember, JsonProperty("message")]
    public string Message { get; set; } = string.Empty;
}

namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class OrderPackageView
{
    [DataMember, JsonProperty("data")]
    public OrderPackageData Data { get; set; } = new();

    [DataMember, JsonProperty("meta")]
    public OrderPackageMeta Meta { get; set; } = new();
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class OrderPackageData
{
    [DataMember, JsonProperty("id")]
    public int Id { get; set; }

    [DataMember, JsonProperty("code")]
    public string Code { get; set; } = string.Empty;

    [DataMember, JsonProperty("package_id")]
    public string PackageId { get; set; } = string.Empty;

    [DataMember, JsonProperty("currency")]
    public string Currency { get; set; } = string.Empty;

    [DataMember, JsonProperty("quantity")]
    public int Quantity { get; set; }

    [DataMember, JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [DataMember, JsonProperty("description")]
    public string? Description { get; set; }

    [DataMember, JsonProperty("esim_type")]
    public string EsimType { get; set; } = string.Empty;

    [DataMember, JsonProperty("validity")]
    public int Validity { get; set; }

    [DataMember, JsonProperty("package")]
    public string Package { get; set; } = string.Empty;

    [DataMember, JsonProperty("data")]
    public string Data { get; set; } = string.Empty;

    [DataMember, JsonProperty("price")]
    public float Price { get; set; }

    [DataMember, JsonProperty("text")]
    public string? Text { get; set; }

    [DataMember, JsonProperty("voice")]
    public string? Voice { get; set; }

    [DataMember, JsonProperty("net_price")]
    public float NetPrice { get; set; }

    [DataMember, JsonProperty("created_at")]
    public string CreatedAt { get; set; } = string.Empty;

    [DataMember, JsonProperty("manual_installation")]
    public string ManualInstallation { get; set; } = string.Empty;

    [DataMember, JsonProperty("qrcode_installation")]
    public string QrCodeInstallation { get; set; } = string.Empty;

    [DataMember, JsonProperty("installation_guides")]
    public InstallationGuides InstallationGuides { get; set; } = new();

    [DataMember, JsonProperty("status")]
    public OrderStatus Status { get; set; } = new();

    [DataMember, JsonProperty("user")]
    public OrderUser User { get; set; } = new();

    [DataMember, JsonProperty("sims")]
    public List<OrderSim> Sims { get; set; } = new();
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class OrderPackageMeta
{
    [DataMember, JsonProperty("message")]
    public string Message { get; set; } = string.Empty;
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class InstallationGuides
{
    [DataMember, JsonProperty("en")]
    public string En { get; set; } = string.Empty;
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class OrderStatus
{
    [DataMember, JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [DataMember, JsonProperty("slug")]
    public string Slug { get; set; } = string.Empty;
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class OrderUser
{
    [DataMember, JsonProperty("id")]
    public int Id { get; set; }

    [DataMember, JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [DataMember, JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    [DataMember, JsonProperty("mobile")]
    public string? Mobile { get; set; }

    [DataMember, JsonProperty("address")]
    public string Address { get; set; } = string.Empty;

    [DataMember, JsonProperty("state")]
    public string? State { get; set; }

    [DataMember, JsonProperty("city")]
    public string City { get; set; } = string.Empty;

    [DataMember, JsonProperty("postal_code")]
    public string PostalCode { get; set; } = string.Empty;

    [DataMember, JsonProperty("country_id")]
    public int CountryId { get; set; }

    [DataMember, JsonProperty("company")]
    public string? Company { get; set; }

    [DataMember, JsonProperty("created_at")]
    public string CreatedAt { get; set; } = string.Empty;
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class OrderSim
{
    [DataMember, JsonProperty("id")]
    public int Id { get; set; }

    [DataMember, JsonProperty("created_at")]
    public string CreatedAt { get; set; } = string.Empty;

    [DataMember, JsonProperty("iccid")]
    public string Iccid { get; set; } = string.Empty;

    [DataMember, JsonProperty("lpa")]
    public string Lpa { get; set; } = string.Empty;

    [DataMember, JsonProperty("imsis")]
    public string? Imsis { get; set; }

    [DataMember, JsonProperty("matching_id")]
    public string MatchingId { get; set; } = string.Empty;

    [DataMember, JsonProperty("qrcode")]
    public string Qrcode { get; set; } = string.Empty;

    [DataMember, JsonProperty("qrcode_url")]
    public string QrcodeUrl { get; set; } = string.Empty;

    [DataMember, JsonProperty("airalo_code")]
    public string? AiraloCode { get; set; }

    [DataMember, JsonProperty("apn_type")]
    public string ApnType { get; set; } = string.Empty;

    [DataMember, JsonProperty("apn_value")]
    public string? ApnValue { get; set; }

    [DataMember, JsonProperty("is_roaming")]
    public bool IsRoaming { get; set; }

    [DataMember, JsonProperty("confirmation_code")]
    public string? ConfirmationCode { get; set; }

    [DataMember, JsonProperty("apn")]
    public ApnSettings Apn { get; set; } = new();

    [DataMember, JsonProperty("msisdn")]
    public string? Msisdn { get; set; }

    [DataMember, JsonProperty("direct_apple_installation_url")]
    public string DirectAppleInstallationUrl { get; set; } = string.Empty;

    [DataMember, JsonProperty("voucher_code")]
    public string? VoucherCode { get; set; }
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ApnSettings
{
    [DataMember, JsonProperty("ios")]
    public ApnPlatformSettings Ios { get; set; } = new();

    [DataMember, JsonProperty("android")]
    public ApnPlatformSettings Android { get; set; } = new();
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ApnPlatformSettings
{
    [DataMember, JsonProperty("apn_type")]
    public string ApnType { get; set; } = string.Empty;

    [DataMember, JsonProperty("apn_value")]
    public string? ApnValue { get; set; }
}
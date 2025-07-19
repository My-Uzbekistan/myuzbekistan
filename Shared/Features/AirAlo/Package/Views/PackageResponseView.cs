namespace myuzbekistan.Shared;


[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponseView
{
    [DataMember, JsonProperty("data")]
    public List<PackageResponseDatum> Data { get; set; } = [];
    [DataMember, JsonProperty("links")]
    public PackageResponseLinks Links { get; set; } = new();
    [DataMember, JsonProperty("meta")]
    public PackageResponseMeta Meta { get; set; } = new();
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponseLinks
{
    [DataMember, JsonProperty("first")]
    public string First { get; set; } = string.Empty;
    [DataMember, JsonProperty("last")]
    public string Last { get; set; } = string.Empty;
    [DataMember, JsonProperty("prev"), MemoryPackAllowSerialize]
    public string? Prev { get; set; } // Nullable to allow for no previous page
    [DataMember, JsonProperty("next"), MemoryPackAllowSerialize]
    public string? Next { get; set; } // Nullable to allow for no next page
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponseMeta
{
    [DataMember, JsonProperty("message")]
    public string Message { get; set; } = string.Empty;
    [DataMember, JsonProperty("current_page")]
    public int CurrentPage { get; set; }
    [DataMember, JsonProperty("from")]
    public int From { get; set; }
    [DataMember, JsonProperty("last_page")]
    public int LastPage { get; set; }
    [DataMember, JsonProperty("path")]
    public string Path { get; set; } = string.Empty;
    [DataMember, JsonProperty("per_page")]
    public string PerPage { get; set; } = string.Empty;
    [DataMember, JsonProperty("to")]
    public int To { get; set; }
    [DataMember, JsonProperty("total")]
    public int Total { get; set; }
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponseDatum
{
    [DataMember, JsonProperty("slug")]
    public string Slug { get; set; } = string.Empty;
    [DataMember, JsonProperty("country_code")]
    public string CountryCode { get; set; } = string.Empty;
    [DataMember, JsonProperty("title")]
    public string Title { get; set; } = string.Empty;
    [DataMember, JsonProperty("image")]
    public PackageResponseImage Image { get; set; } = new();
    [DataMember, JsonProperty("operators")]
    public List<PackageResponseOperator> Operators { get; set; } = [];
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponseImage
{
    [DataMember, JsonProperty("width")]
    public int Width { get; set; }
    [DataMember, JsonProperty("height")]
    public int Height { get; set; }
    [DataMember, JsonProperty("url")]
    public string Url { get; set; } = string.Empty;
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponseOperator
{
    [DataMember, JsonProperty("id")]
    public int Id { get; set; }
    [DataMember, JsonProperty("style")]
    public string Style { get; set; } = string.Empty;
    [DataMember, JsonProperty("gradient_start")]
    public string GradientStart { get; set; } = string.Empty;
    [DataMember, JsonProperty("gradient_end")]
    public string GradientEnd { get; set; } = string.Empty;
    [DataMember, JsonProperty("type")]
    public string Type { get; set; } = string.Empty;
    [DataMember, JsonProperty("title")]
    public string Title { get; set; } = string.Empty;
    [DataMember, JsonProperty("is_prepaid")]
    public bool IsPrepaid { get; set; }
    [DataMember, JsonProperty("esim_type")]
    public string EsimType { get; set; } = string.Empty;
    [DataMember, JsonProperty("warning")]
    public string? Warning { get; set; }
    [DataMember, JsonProperty("apn_type")]
    public string ApnType { get; set; } = string.Empty;
    [DataMember, JsonProperty("apn_value")]
    public string? ApnValue { get; set; }
    [DataMember, JsonProperty("is_roaming")]
    public bool IsRoaming { get; set; }
    [DataMember, JsonProperty("info")]
    public List<string> Info { get; set; } = [];
    [DataMember, JsonProperty("image")]
    public PackageResponseImage1 Image { get; set; } = new();
    [DataMember, JsonProperty("plan_type")]
    public string PlanType { get; set; } = string.Empty;
    [DataMember, JsonProperty("is_kyc_verify")]
    public bool IsKycVerify { get; set; }
    [DataMember, JsonProperty("activation_policy")]
    public string ActivationPolicy { get; set; } = string.Empty;
    [DataMember, JsonProperty("rechargeability")]
    public bool Rechargeability { get; set; }
    [DataMember, JsonProperty("other_info")]
    public string? OtherInfo { get; set; }
    [DataMember, JsonProperty("coverages")]
    public List<PackageResponseCoverage> Coverages { get; set; } = [];
    [DataMember, JsonProperty("install_window_days"), MemoryPackAllowSerialize]
    public string? InstallWindowDays { get; set; }
    [DataMember, JsonProperty("topup_grace_window_days"), MemoryPackAllowSerialize]
    public string? TopupGraceWindowDays { get; set; }
    [DataMember, JsonProperty("apn")]
    public PackageResponseApn Apn { get; set; } = new();
    [DataMember, JsonProperty("packages")]
    public List<PackageResponsePackage> Packages { get; set; } = [];
    [DataMember, JsonProperty("countries")]
    public List<PackageResponseCountry> Countries { get; set; } = [];
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponseImage1
{
    [DataMember, JsonProperty("width")]
    public int Width { get; set; }
    [DataMember, JsonProperty("height")]
    public int Height { get; set; }
    [DataMember, JsonProperty("url")]
    public string Url { get; set; } = string.Empty;
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponseApn
{
    [DataMember, JsonProperty("ios")]
    public PackageResponseIos Ios { get; set; } = new();
    [DataMember, JsonProperty("android")]
    public PackageResponseAndroid Android { get; set; } = new();
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponseIos
{
    [DataMember, JsonProperty("apn_type")]
    public string ApnType { get; set; } = string.Empty;
    [DataMember, JsonProperty("apn_value")]
    public string? ApnValue { get; set; }
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponseAndroid
{
    [DataMember, JsonProperty("apn_type")]
    public string ApnType { get; set; } = string.Empty;
    [DataMember, JsonProperty("apn_value")]
    public string? ApnValue { get; set; }
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponseCoverage
{
    [DataMember, JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    [DataMember, JsonProperty("code")]
    public string Code { get; set; } = string.Empty;
    [DataMember, JsonProperty("networks")]
    public List<PackageResponseNetwork> Networks { get; set; } = [];
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponseNetwork
{
    [DataMember, JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [DataMember, JsonProperty("types")]
    public List<string> Types { get; set; } = [];
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponsePackage
{
    [DataMember, JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [DataMember, JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [DataMember, JsonProperty("price")]
    public float Price { get; set; }

    [DataMember, JsonProperty("amount")]
    public int Amount { get; set; }

    [DataMember, JsonProperty("day")]
    public int Day { get; set; }

    [DataMember, JsonProperty("is_unlimited")]
    public bool IsUnlimited { get; set; }

    [DataMember, JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    [DataMember, JsonProperty("short_info")]
    public string ShortInfo { get; set; } = string.Empty;

    [DataMember, JsonProperty("qr_installation")]
    public string QrInstallation { get; set; } = string.Empty;

    [DataMember, JsonProperty("manual_installation")]
    public string ManualInstallation { get; set; } = string.Empty;

    [DataMember, JsonProperty("is_fair_usage_policy")]
    public bool IsFairUsagePolicy { get; set; }

    [DataMember, JsonProperty("fair_usage_policy")]
    public string? FairUsagePolicy { get; set; }

    [DataMember, JsonProperty("data")]
    public string Data { get; set; } = string.Empty;

    [DataMember, JsonProperty("voice")]
    public string? Voice { get; set; }

    [DataMember, JsonProperty("text")]
    public string? Text { get; set; }

    [DataMember, JsonProperty("net_price")]
    public float NetPrice { get; set; }

    [DataMember, JsonProperty("prices")]
    public PackageResponsePrices Prices { get; set; } = default!;
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponsePrices
{
    [DataMember, JsonProperty("net_price")]
    public PackageResponseNetPrice NetPrice { get; set; } = default!;

    [DataMember, JsonProperty("recommended_retail_price")]
    public PackageResponseRecommendedRetailPrice RecommendedRetailPrice { get; set; } = default!;
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponseNetPrice
{
    [property: DataMember] public float AUD { get; set; }

    [property: DataMember] public float BRL { get; set; }

    [property: DataMember] public float GBP { get; set; }

    [property: DataMember] public float AED { get; set; }

    [property: DataMember] public float EUR { get; set; }

    [property: DataMember] public float ILS { get; set; }

    [property: DataMember] public int JPY { get; set; }

    [property: DataMember] public float MXN { get; set; }

    [property: DataMember] public float USD { get; set; }

    [property: DataMember] public int VND { get; set; }
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponseRecommendedRetailPrice
{
    [property: DataMember] public float AUD { get; set; }

    [property: DataMember] public float BRL { get; set; }

    [property: DataMember] public float GBP { get; set; }

    [property: DataMember] public float AED { get; set; }

    [property: DataMember] public float EUR { get; set; }

    [property: DataMember] public float ILS { get; set; }

    [property: DataMember] public int JPY { get; set; }

    [property: DataMember] public float MXN { get; set; }

    [property: DataMember] public float USD { get; set; }

    [property: DataMember] public int VND { get; set; }
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponseCountry
{
    [DataMember, JsonProperty("country_code")]
    public string CountryCode { get; set; } = string.Empty;

    [DataMember, JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    [DataMember, JsonProperty("image")]
    public PackageResponseImage2 Image { get; set; } = default!;
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class PackageResponseImage2
{
    [DataMember, JsonProperty("width")]
    public int Width { get; set; }

    [DataMember, JsonProperty("height")]
    public int Height { get; set; }

    [DataMember, JsonProperty("url")]
    public string Url { get; set; } = string.Empty;
}

using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class Currency
{
    [property: DataMember] public int Id { get; set; }
    [property: DataMember] public string Ccy { get; set; } = null!;
    [property: DataMember] public string Rate { get; set; } = null!;

    [property: DataMember] public string Title { get; set; } = null!;
}


public partial class CurrencyRaw
{
    public int Id { get; set; }
    public string Ccy { get; set; } = null!;
    public string Rate { get; set; } = null!;

    [JsonPropertyName("CcyNm_RU")] public string CcyNm_RU { get; set; } = null!;
    [JsonPropertyName("CcyNm_UZ")] public string CcyNm_UZ { get; set; } = null!;
    [JsonPropertyName("CcyNm_EN")] public string CcyNm_EN { get; set; } = null!;

    public string Title => LangHelper.currentLocale switch
    {
        "ru-RU" => CcyNm_RU,
        "uz-UZ" => CcyNm_UZ,
        "en-US" => CcyNm_EN,
        _ => CcyNm_RU
    };


}

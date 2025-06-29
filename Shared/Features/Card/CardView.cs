using MemoryPack;
using ActualLab.Fusion.Blazor;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class  CardView
{
   [property: DataMember, JsonPropertyName("card_id")] public long CardId { get; set; }
   [property: DataMember, JsonPropertyName("user_id")] public long UserId { get; set; }
   [property: DataMember, JsonPropertyName("expiration_date")] public string? ExpirationDate { get; set; } 
   [property: DataMember, JsonPropertyName("application_id")] public int ApplicationId { get; set; }
   [property: DataMember, JsonPropertyName("payer_id")] public string PayerId { get; set; } = null!;
   [property: DataMember, JsonPropertyName("card_pan")] public string CardPan { get; set; } = null!;
   [property: DataMember, JsonPropertyName("card_token")] public string CardToken { get; set; } = null!;
   [property: DataMember, JsonPropertyName("phone")] public string Phone { get; set; } = null!;
   [property: DataMember, JsonPropertyName("holder_name")] public string HolderName { get; set; } = null!;
   [property: DataMember, JsonPropertyName("pinfl")] public string? Pinfl { get; set; } 
   [property: DataMember, JsonPropertyName("ps")] public string Ps { get; set; } = null!;
   [property: DataMember, JsonPropertyName("status")] public string Status { get; set; } = null!;
   [property: DataMember, JsonPropertyName("added_on")] public string AddedOn { get; set; } = null!;
   [property: DataMember, JsonPropertyName("card_status")] public CardStatus? CardStatus { get; set; } 
   [property: DataMember, JsonPropertyName("sms_inform")] public bool? SmsInform { get; set; } 
   [property: DataMember, JsonPropertyName("is_multicard")] public bool IsMulticard { get; set; } 
   [property: DataMember, JsonPropertyName("id")] public long Id { get; set; }

    [property: DataMember, JsonPropertyName("code")] public CardColorView? Code { get; set; } = null!;
    [property: DataMember, JsonPropertyName("name")] public string? Name { get; set; } = null!;

    [property: DataMember, JsonPropertyName("cvv")] public string? Cvv { get; set; } = null!;

    public override bool Equals(object? o)
    {
        var other = o as CardView;
        return other?.Id == Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
}



[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class CardInfo
{
    [property: DataMember, JsonPropertyName("user_id")] public long UserId { get; set; }
    [property: DataMember, JsonPropertyName("expiration_date")] public string? ExpirationDate { get; set; }
    //[property: DataMember, JsonPropertyName("phone")] public string Phone { get; set; } = null!;
    [property: DataMember, JsonPropertyName("holder_name")] public string HolderName { get; set; } = null!;
    [property: DataMember, JsonPropertyName("card_number")] public string CardNumber { get; set; } = null!;
    [property: DataMember, JsonPropertyName("name")] public string Name { get; set; } = null!;
    [property: DataMember, JsonPropertyName("color_code")] public string ColorCode { get; set; } = null!;
    [property: DataMember, JsonPropertyName("status")] public string Status { get; set; } = null!;
    [property: DataMember, JsonPropertyName("card_status")] public CardStatus? CardStatus { get; set; }
    [property: DataMember, JsonPropertyName("balance")] public decimal Balance { get; set; }
    [property: DataMember, JsonPropertyName("id")] public long Id { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as CardView;
        return other?.Id == Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
}


[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class CardStatus
{
    [property: DataMember, JsonPropertyName("code")] public int Code { get; set; } 
    [property: DataMember, JsonPropertyName("description")] public string Description { get; set; } = null!;
}
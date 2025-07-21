using ActualLab.Fusion.Blazor;
using MemoryPack;
using Microsoft.VisualBasic;
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
   [property: DataMember, JsonPropertyName("card_pan")] public string CardPan { get; set; } = null!;
   [property: DataMember, JsonPropertyName("card_token")] public string CardToken { get; set; } = null!;
   [property: DataMember, JsonPropertyName("phone")] public string Phone { get; set; } = null!;
   [property: DataMember, JsonPropertyName("holder_name")] public string HolderName { get; set; } = null!;
   [property: DataMember, JsonPropertyName("pinfl")] public string? Pinfl { get; set; } 
   [property: DataMember, JsonPropertyName("ps")] public string Ps { get; set; } = null!;
   [property: DataMember, JsonPropertyName("status")] public string? Status { get; set; } = null!;
   [property: DataMember, JsonPropertyName("id")] public long Id { get; set; }

    [property: DataMember, JsonPropertyName("balance")] public decimal? Balance { get; set; } = null!;
    [property: DataMember, JsonPropertyName("cvv")] public int? Cvv { get; set; }

    [property: DataMember, JsonPropertyName("is_external")] public bool IsExternal
    {
        get => Ps == "VISA" || Ps == "MASTERCARD";
        set { /* Needed for serialization, but can be left empty or throw if you want to make it read-only */ }
    }
    [property: DataMember, JsonPropertyName("icon")] public string? Icon { get; set; } = null!;

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
    //[property: DataMember, JsonPropertyName("user_id")] public long UserId { get; set; }
    //[property: DataMember, JsonPropertyName("expiration_date")] public string? ExpirationDate { get; set; }
    //[property: DataMember, JsonPropertyName("phone")] public string Phone { get; set; } = null!;
    //[property: DataMember, JsonPropertyName("holder_name")] public string HolderName { get; set; } = null!;
    [property: DataMember, JsonPropertyName("card_number")] public string CardNumber { get; set; } = null!;
    [property: DataMember, JsonPropertyName("status")] public string Status { get; set; } = null!;
    [property: DataMember, JsonPropertyName("status_message")] public string StatusMessage { get; set; } = null!;
    [property: DataMember, JsonPropertyName("ps")] public string Ps { get; set; } = null!;
    [property: DataMember, JsonPropertyName("id")] public long Id { get; set; }
    [property: DataMember, JsonPropertyName("is_external")] public bool IsExternal
    {
        get => Ps == "VISA" || Ps == "MASTERCARD";
        set { /* Needed for serialization, but can be left empty or throw if you want to make it read-only */ }
    }
    [property: DataMember, JsonPropertyName("icon")] public string? Icon { get => CardHelper.GetCardBrandImage(Ps); set { } } 

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
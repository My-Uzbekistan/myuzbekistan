using System.Text.Json.Serialization;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class CardPrefixView
{
    [property: DataMember] public uint Prefix { get; set; }
    [property: DataMember] public string BankName { get; set; } = null!;
    [property: DataMember] public string CardType { get; set; } = null!;
    [property: DataMember] public FileView? CardBrandView { get; set; } 
    [property: DataMember] public long Id { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as CardPrefixView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class CardPrefixApi
{
    [property: DataMember] public uint Prefix { get; set; }
    [property: DataMember] public string BankName { get; set; } = null!;
    [property: DataMember] public string CardType { get; set; } = null!;
    [property: DataMember] public string? CardBrand { get; set; }

}
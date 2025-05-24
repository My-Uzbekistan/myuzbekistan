using ActualLab.Fusion.Blazor;
using MemoryPack;
using myuzbekistan.Shared;
using System.Runtime.Serialization;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class MerchantView
{
    [property: DataMember] public string? Name { get; set; }
    [property: DataMember] public string? Phone { get; set; }
    [property: DataMember] public string? Email { get; set; }
    [property: DataMember] public string? Address { get; set; }
    [property: DataMember] public string? Description { get; set; }
    [property: DataMember] public string? Contract { get; set; }
    [property: DataMember] public string Inn { get; set; } = null!;
    [property: DataMember] public string? Mfi { get; set; }
    [property: DataMember] public string CurrentAccount { get; set; } = null!;
    [property: DataMember] public short Discount { get; set; }
    [property: DataMember] public bool IsVat { get; set; } 
    [property: DataMember] public FileView? ImageView { get; set; } 
    [property: DataMember] public bool IsActive { get; set; } 
    [property: DataMember] public MerchantView? ParentView { get; set; } 
    [property: DataMember] public long Id { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as MerchantView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
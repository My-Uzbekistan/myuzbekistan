
using Point = NetTopologySuite.Geometries.Point;
[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class MerchantCategoryView
{
    [property: DataMember] public string Locale { get; set; } = null!;
    [property: DataMember] public FileView? LogoView { get; set; } 
    [property: DataMember] public string? BrandName { get; set; }
    [property: DataMember] public string? OrganizationName { get; set; }
    [property: DataMember] public string? Description { get; set; }
    [property: DataMember] public string Inn { get; set; } = null!;
    [property: DataMember] public string AccountNumber { get; set; } = null!;
    [property: DataMember] public string? MfO { get; set; }
    [property: DataMember] public string? Contract { get; set; }
    [property: DataMember] public short Discount { get; set; }
    [property: DataMember] public Byte PayDay { get; set; }
    [property: DataMember, Required] public ServiceTypeView ServiceType { get; set; } = null!;  
    [property: DataMember] public string? Phone { get; set; }
    [property: DataMember] public string? Email { get; set; }
    [property: DataMember] public string? Address { get; set; }
    [property: DataMember] public bool IsVat { get; set; } 
    [property: DataMember] public Byte Vat { get; set; } 
    [property: DataMember] public bool Status { get; set; }
    [property: DataMember] public ICollection<MerchantView> MerchantsView { get; set; } = new List<MerchantView>();
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public string? Token { get; set; }

    [property: DataMember] public List<string?> ChatIds { get; set; } = new List<string?>();

    public override bool Equals(object? o)
    {
        var other = o as MerchantCategoryView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
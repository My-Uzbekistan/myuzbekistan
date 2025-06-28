using Point = NetTopologySuite.Geometries.Point;
[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class MerchantView
{
    [property: DataMember] public string Locale { get; set; } = null!;
    [property: DataMember] public FileView? LogoView { get; set; } 
    [property: DataMember] public string? Name { get; set; }
    [property: DataMember] public string? Description { get; set; }
    [property: DataMember] public string? Address { get; set; }
    [property: DataMember] public string? MXIK { get; set; }
    [property: DataMember] public string? WorkTime { get; set; }
    [property: DataMember] public string? Phone { get; set; }
    [property: DataMember] public string Responsible { get; set; } = null!;
    [property: DataMember] public bool Status { get; set; }
    [property: DataMember] public MerchantCategoryView? MerchantCategoryView { get; set; } 
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public string? Token { get; set; }
    [property: DataMember] public List<string?> ChatIds { get; set; } = new List<string?>();
    [property: DataMember, MemoryPackAllowSerialize, JsonConverter(typeof(NetTopologySuite.IO.Converters.GeometryConverter))] public Point? Location { get; set; }


    public override bool Equals(object? o)
    {
        var other = o as MerchantView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
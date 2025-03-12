using ActualLab.Fusion.Blazor;
using MemoryPack;
using Microsoft.EntityFrameworkCore;
using Shared.Localization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Point = NetTopologySuite.Geometries.Point;

namespace myuzbekistan.Shared;

[PrimaryKey(nameof(ContentEntity.Id), nameof(ContentEntity.Locale))]
public class ContentEntity : BaseEntity
{
    
    public string Title { get; set; } = null!;
    public string? Description { get; set; } 
    public long CategoryId { get; set; }
    public string CategoryLocale { get; set; } = null!;
    public CategoryEntity Category { get; set; } = null!;
    public string? WorkingHours { get; set; } 
    public List<FacilityEntity>? Facilities { get; set; } = [];
    public Point? Location { get; set; }
    [Column(TypeName = "jsonb")]
    public List<CallInformation>? Contacts { get; set; } = [];
    [InverseProperty("ContentFiles")]
    public ICollection<FileEntity>? Files { get; set; } = [];
    [InverseProperty("ContentPhoto")]
    public FileEntity? Photo { get; set; }
    public long? PhotoId { get; set; }
    [InverseProperty("ContentPhotos")]
    public ICollection<FileEntity>? Photos { get; set; } = [];
    public ICollection<ReviewEntity>? Reviews { get; set; } = [];
    public ICollection<LanguageEntity>? Languages { get; set; } = [];
    public RegionEntity? Region { get; set; } = null!;
    public double RatingAverage { get; set; } 
    public int AverageCheck { get; set; } 
    public decimal Price { get; set; }
    public decimal PriceInDollar { get; set; }
    public string? Address { get; set; } = null!;

    public bool Recommended { get; set; } = false;
    public string Locale { get; set; } = null!;
    public ContentStatus Status { get; set; } = ContentStatus.Active;

}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class CallInformation
{
    [property: DataMember] public string Contact { get; set; } = null!;

    [property: DataMember] public string Action => Icon switch
    {
        "/Images/phone.svg" => "tel:" + Contact,
        "/Images/telegram.svg" => "https://t.me/" + Contact.Replace("@",""),
        "/Images/instagram.svg" => "https://instagram.com/" + Contact.Replace("@",""),
        "/Images/whatsapp.svg" => "https://wa.me/" + Regex.Replace(Contact, @"\D", ""),
        _ => Contact
    };
    [property: DataMember] public string Name { get; set; } = null!;
    [property: DataMember] public string Icon { get; set; } = null!;
}
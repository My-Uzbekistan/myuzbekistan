using Microsoft.EntityFrameworkCore;

namespace myuzbekistan.Shared;

[PrimaryKey(nameof(MerchantCategoryEntity.Id), nameof(MerchantCategoryEntity.Locale))]
public class MerchantCategoryEntity : BaseEntity
{
    public string Locale { get; set; } = null!;
    public FileEntity? Logo { get; set; } = null!;
    public string? BrandName { get; set; } = null!;
    public string? OrganizationName { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public string Inn { get; set; } = null!;
    public string AccountNumber { get; set; } = null!;
    public string? MfO { get; set; } = null!;
    public string? Contract { get; set; } = null!;
    public short Discount { get; set; } = 0;
    public byte PayDay { get; set; }
    public ServiceTypeEntity ServiceType { get; set; } = null!;
    public string? Phone { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public string? Address { get; set; } = null!;
    public bool IsVat { get; set; }
    public Byte Vat { get; set; }
    public bool Status { get; set; } = false;

    public string? Token { get; set; }
    public List<string?> ChatIds { get; set; } = new List<string?>();
    public  List<MerchantEntity> Merchants { get; set; } = new List<MerchantEntity>();

}

namespace myuzbekistan.Shared;

public class MerchantCategoryEntity : BaseEntity
{
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
    public string ServiceType { get; set; } = null!;
    public string? Phone { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public string? Address { get; set; } = null!;
    public bool IsVat { get; set; }
    public bool Status { get; set; } = false;
    public  List<MerchantEntity> Merchants { get; set; } = new List<MerchantEntity>();

}

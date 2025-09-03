namespace myuzbekistan.Shared;

public class ESimPromoCodeEntity : BaseEntity
{
    public bool IsCompatibleWithDiscount { get; set; } = false;
    public string Code { get; set; } = string.Empty;
    public PromoCodeType PromoCodeType { get; set; }
    public int UsageLimit { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DiscountType DiscountType { get; set; }
    public double DiscountValue { get; set; }
    public int AppliedCount { get; set; } = 0;
    public int MaxUsagePerUser { get; set; } = 1;

    public virtual ICollection<ESimOrderEntity> ESimOrderEntities { get; set; } = [];
}
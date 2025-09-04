namespace myuzbekistan.Shared;

public class ESimPromoCodeUsageEntity : BaseEntity
{
    public long PromoCodeId { get; set; }
    public long ApplicationUserId { get; set; }
    public long ESimOrderId { get; set; }
}
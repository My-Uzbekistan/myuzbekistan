namespace myuzbekistan.Shared;

public class ESimOrderEntity : BaseEntity
{
    #region Order data
    public int OrderId { get; set; }
    public string OrderCode { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string EsimType { get; set; } = string.Empty;
    public string Package { get; set; } = string.Empty;
    public string PackageId { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public float Price { get; set; }
    public int Validity { get; set; }
    #endregion

    #region Sim data
    public int SimId { get; set; }
    public DateTime SimCreatedAt { get; set; }
    public string Iccid { get; set; } = string.Empty;
    public string Lpa { get; set; } = string.Empty;
    public string MatchingId { get; set; } = string.Empty;
    public string? ConfirmationCode { get; set; }
    public string QrCode { get; set; } = string.Empty;
    public string QrCodeUrl { get; set; } = string.Empty;
    public string DirectAppleUrl { get; set; } = string.Empty;
    public string ManualInstallation { get; set; } = string.Empty;
    public string QrCodeInstallation { get; set; } = string.Empty;
    #endregion
    public DateTime? ActivationDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public double CustomPrice { get; set; }
    public double? DiscountPercentage { get; set; }
    public long UserId { get; set; }
    public long? PromoCodeId { get; set; }
    public ESimPromoCodeEntity? ESimPromoCodeEntity { get; set; }
}
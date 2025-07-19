namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ESimOrderView
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public long UserId { get; set; }

    #region Order data

    [property: DataMember] public int OrderId { get; set; }
    [property: DataMember] public string OrderCode { get; set; } = string.Empty;
    [property: DataMember] public string Currency { get; set; } = string.Empty;
    [property: DataMember] public string Type { get; set; } = string.Empty;
    [property: DataMember] public string EsimType { get; set; } = string.Empty;
    [property: DataMember] public string Package { get; set; } = string.Empty;
    [property: DataMember] public string PackageId { get; set; } = string.Empty;
    [property: DataMember] public string Data { get; set; } = string.Empty;
    [property: DataMember] public float Price { get; set; }
    [property: DataMember] public int Validity { get; set; }

    #endregion

    #region Sim data

    [property: DataMember] public int SimId { get; set; }
    [property: DataMember] public DateTime SimCreatedAt { get; set; }
    [property: DataMember] public string Iccid { get; set; } = string.Empty;
    [property: DataMember] public string Lpa { get; set; } = string.Empty;
    [property: DataMember] public string MatchingId { get; set; } = string.Empty;
    [property: DataMember] public string? ConfirmationCode { get; set; }
    [property: DataMember] public string QrCode { get; set; } = string.Empty;
    [property: DataMember] public string QrCodeUrl { get; set; } = string.Empty;
    [property: DataMember] public string DirectAppleUrl { get; set; } = string.Empty;
    [property: DataMember] public string ManualInstallation { get; set; } = string.Empty;
    [property: DataMember] public string QrCodeInstallation { get; set; } = string.Empty;

    #endregion

    [property: DataMember] public double CustomPrice { get; set; }
    [property: DataMember] public double? DiscountPercentage { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as ESimOrderView;
        return other?.Id == Id &&
               other.OrderId == OrderId &&
               other.PackageId == PackageId &&
               other.UserId == UserId &&
               other.SimCreatedAt == SimCreatedAt;
    }

    public override int GetHashCode()
        => HashCode.Combine(Id, OrderId, PackageId, UserId, SimCreatedAt);

    public static implicit operator ESimOrderView(OrderPackageView src)
        => new()
        {
            // Order data
            OrderId = src.Data.Id,
            OrderCode = src.Data.Code,
            Currency = src.Data.Currency,
            Type = src.Data.Type,
            EsimType = src.Data.EsimType,
            Package = src.Data.Package,
            PackageId = src.Data.PackageId,
            Data = src.Data.Data,
            Price = src.Data.Price,
            Validity = src.Data.Validity,

            // Sim data (use first sim if available)
            SimId = src.Data.Sims.Count > 0 ? src.Data.Sims[0].Id : 0,
            SimCreatedAt = src.Data.Sims.Count > 0 && DateTime.TryParse(src.Data.Sims[0].CreatedAt, out var simCreated) ? simCreated : default,
            Iccid = src.Data.Sims.Count > 0 ? src.Data.Sims[0].Iccid : string.Empty,
            Lpa = src.Data.Sims.Count > 0 ? src.Data.Sims[0].Lpa : string.Empty,
            MatchingId = src.Data.Sims.Count > 0 ? src.Data.Sims[0].MatchingId : string.Empty,
            ConfirmationCode = src.Data.Sims.Count > 0 ? src.Data.Sims[0].ConfirmationCode : null,
            QrCode = src.Data.Sims.Count > 0 ? src.Data.Sims[0].Qrcode : string.Empty,
            QrCodeUrl = src.Data.Sims.Count > 0 ? src.Data.Sims[0].QrcodeUrl : string.Empty,
            DirectAppleUrl = src.Data.Sims.Count > 0 ? src.Data.Sims[0].DirectAppleInstallationUrl : string.Empty,
            ManualInstallation = src.Data.ManualInstallation,
            QrCodeInstallation = src.Data.QrCodeInstallation,
        };
}                          
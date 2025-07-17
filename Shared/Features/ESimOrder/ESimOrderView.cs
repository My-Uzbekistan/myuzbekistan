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
}
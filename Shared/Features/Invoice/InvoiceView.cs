namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class InvoiceView
{
    [property: DataMember] public Decimal Amount { get; set; }
    [property: DataMember] public string? Currency { get; set; } = "UZS";
    [property: DataMember] public string? Description { get; set; }
    [property: DataMember] public ApplicationUserView? User { get; set; } = null!;
    [property: DataMember] public MerchantView MerchantView { get; set; } = null!;
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public DateTime CreatedAt { get; set; }
    [property: DataMember] public string ExternalId { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as InvoiceView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}

public class InvoiceSummaryView
{
    public DateTime Date { get; set; }
    public string PaymentId { get; set; } = string.Empty;
    public int Amount { get; set; }
    public MerchantData Merchant { get; set; } = null!;
}

public class InvoiceDetailView : InvoiceSummaryView
{
    public List<object> Items { get; set; } = new List<object>();
}

public class MerchantData
{
    public string Icon { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

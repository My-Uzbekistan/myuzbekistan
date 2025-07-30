namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class MyEsimsView
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public string CountryName { get; set; } = string.Empty;
    [property: DataMember] public string OperatorName { get; set; } = string.Empty;
    [property: DataMember] public string DataValume { get; set; } = string.Empty;
    [property: DataMember] public int Voice { get; set; }
    [property: DataMember] public int Text { get; set; }
    [property: DataMember] public bool HasVoicePack { get; set; }
    [property: DataMember] public double RemainingData { get; set; }
    [property: DataMember] public DateTime? ActivationDate { get; set; }
    [property: DataMember] public int ValidDays { get; set; }
    [property: DataMember] public string ImageUrl { get; set; } = string.Empty;

    public override bool Equals(object? o)
    {
        var other = o as MyEsimsView;
        return other?.Id == Id &&
                other?.CountryName == CountryName &&
                other?.OperatorName == OperatorName &&
                other?.DataValume == DataValume &&
                other?.RemainingData == RemainingData &&
                other?.ActivationDate == ActivationDate &&
                other?.ValidDays == ValidDays;
    }

    public override int GetHashCode()
        => HashCode.Combine(Id, CountryName, OperatorName, DataValume, RemainingData, ActivationDate, ValidDays);
}
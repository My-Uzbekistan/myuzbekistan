[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class SmsTemplateView
{
    [property: DataMember] public string Locale { get; set; } = null!;
    [property: DataMember] public string Template { get; set; } = null!;
    [property: DataMember] public string Key { get; set; } = null!;
    [property: DataMember] public long Id { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as SmsTemplateView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
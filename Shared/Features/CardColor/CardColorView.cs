[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class CardColorView
{
    [property: DataMember] public FileView? ImageView { get; set; } 
    [property: DataMember] public long Id { get; set; }

    public override bool Equals(object? o)
    {
        var other = o as CardColorView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}


[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class CardColorViewApi
{
    [property: DataMember] public string? Image { get; set; }
}
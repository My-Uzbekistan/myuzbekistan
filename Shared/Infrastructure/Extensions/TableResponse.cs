namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
public partial class TableResponse<T> where T : class
{
    [property: DataMember] public IEnumerable<T> Items { get; set; } = new List<T>();

    [property: DataMember] public int TotalItems { get; set; }
}
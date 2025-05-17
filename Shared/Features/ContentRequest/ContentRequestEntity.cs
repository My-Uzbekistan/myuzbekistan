using ActualLab.Fusion;
using ActualLab.Fusion.Blazor;
using MemoryPack;
using System.Runtime.Serialization;

namespace myuzbekistan.Shared;

public class ContentRequestEntity : BaseEntity
{
    public long? ContentId { get; set; }
    public string? ContentName { get; set; } = null!;
    public string? ContentLocale { get; set; } = null!;

    public long? CategoryId { get; set; }
    public string? CategoryName { get; set; } = null!;
    public long? UserId { get; set; }

    public long? RegionId { get; set; }
    public string? RegionName { get; set; }
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ContentRequestView
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public long? ContentId { get; set; }
    [property: DataMember] public string? ContentName { get; set; } = null!;
    [property: DataMember] public string? ContentLocale { get; set; } = null!;
    [property: DataMember] public long? CategoryId { get; set; }
    [property: DataMember] public string? CategoryName { get; set; } = null!;
    [property: DataMember] public long? UserId { get; set; }
    [property: DataMember] public long? RegionId { get; set; }
    [property: DataMember] public string? RegionName { get; set; } = null!;
}


[DataContract, MemoryPackable]
public partial record AddRequestCommand([property: DataMember] Session Session, [property: DataMember] ContentRequestView ContentRequest) : ISessionCommand<ContentRequestView>;


// Вспомогательные DTO для возврата статистики
[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class StatisticSummaryView
{
    [property: DataMember] public int CategoryCount { get; set; }
    [property: DataMember] public List<CategoryContentCount> ContentPerCategory { get; set; } = new();
    [property: DataMember] public int UserCount { get; set; }
    [property: DataMember] public List<FavoriteStat> FavoritePerContent { get; set; } = new();
    [property: DataMember] public int FacilityCount { get; set; }
    [property: DataMember] public List<TopRequestedPlace> TopRequestedPlaces { get; set; } = new();
    [property: DataMember] public List<CategoryRequestByDate> RequestsByDate { get; set; } = new();
    [property: DataMember] public double TotalFileSizeInMb { get; set; }
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class CategoryContentCount
{
    [property: DataMember] public long CategoryId { get; set; }
    [property: DataMember] public string? CategoryName { get; set; }
    [property: DataMember] public int ContentCount { get; set; }
    
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class FavoriteStat
{
    [property: DataMember] public long ContentId { get; set; }
    [property: DataMember] public string ContentName { get; set; } = null!;
    [property: DataMember] public int Count { get; set; }
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class TopRequestedPlace
{
    [property: DataMember] public long? CategoryId { get; set; }
    [property: DataMember] public string CategoryName { get; set; } = null!;
    [property: DataMember] public long? ContentId { get; set; }
    [property: DataMember] public string ContentName { get; set; } = null!;
    [property: DataMember] public int RequestCount { get; set; }
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class CategoryRequestByDate
{
    [property: DataMember] public long? CategoryId { get; set; }
    [property: DataMember] public string CategoryName { get; set; } = null!;
    [property: DataMember] public long? ContentId { get; set; }
    [property: DataMember] public string ContentName { get; set; } = null!;
    [property: DataMember] public DateTime Date { get; set; }
    [property: DataMember] public int Count { get; set; }
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class StatisticFilter
{
    [property: DataMember] public DateTime? StartDate { get; set; }
    [property: DataMember] public DateTime? EndDate { get; set; }
}
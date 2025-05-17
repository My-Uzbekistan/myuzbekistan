using ActualLab.Fusion;
using System.Runtime.Serialization;

namespace myuzbekistan.Shared;

public interface IContentStatisticService : IComputeService
{
    Task<StatisticSummaryView> GetSummary(StatisticFilter filter, CancellationToken cancellationToken = default);


    Task<List<ContentRequestView>> GetAll(long CategoryId, TableOptions options, CancellationToken cancellationToken = default);
}
        
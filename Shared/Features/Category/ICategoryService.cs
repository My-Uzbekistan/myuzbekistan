using ActualLab.Async;
using ActualLab.CommandR.Configuration;
using ActualLab.Fusion;
using System.Reactive;

namespace myuzbekistan.Shared;
public interface ICategoryService:IComputeService
{
    [ComputeMethod]
    Task<TableResponse<CategoryView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<List<CategoryView>> Get(long Id, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Create(CreateCategoryCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Update(UpdateCategoryCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Delete(DeleteCategoryCommand command, CancellationToken cancellationToken = default);
    Task<Unit> Invalidate(){ return TaskExt.UnitTask; }

    Task<List<CategoryApi>> GetCategories(CancellationToken cancellationToken = default);

    Task<List<MainPageApi>> GetMainPageApi(TableOptions options,CancellationToken cancellationToken = default,bool isNewApi = false);
}
    
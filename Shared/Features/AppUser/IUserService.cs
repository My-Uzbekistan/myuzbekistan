using ActualLab.Async;
using ActualLab.CommandR.Configuration;
using ActualLab.Fusion;
using myuzbekistan.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace myuzbekistan.Shared;

public interface IUserService:IComputeService
{
    [ComputeMethod]
    Task<TableResponse<ApplicationUser>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<List<ApplicationUser>> Get(long Id, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Create(CreateCategoryCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Update(UpdateCategoryCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Delete(DeleteCategoryCommand command, CancellationToken cancellationToken = default);
    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}

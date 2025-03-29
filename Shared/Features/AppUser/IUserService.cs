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
    Task<Unit> Invalidate() { return TaskExt.UnitTask; }

    [CommandHandler]
    Task<string> UserToExcel(UserToExcelCommand command, CancellationToken cancellationToken = default);
}

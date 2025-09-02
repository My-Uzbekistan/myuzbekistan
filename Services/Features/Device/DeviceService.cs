using myuzbekistan.Services;

public class DeviceService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), IDeviceService 
{
    #region Queries

    [ComputeMethod]
    public async virtual Task<TableResponse<DeviceView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var device = from s in dbContext.Devices select s;

        if (!string.IsNullOrEmpty(options.Search))
        {
            device = device.Where(s => 
                     s.FirebaseToken.Contains(options.Search)
                    || s.OsVersion.Contains(options.Search)
                    || s.Model.Contains(options.Search)
                    || s.AppVersion.Contains(options.Search)
                    || s.Session.Contains(options.Search)
            );
        }

        Sorting(ref device, options);
        
        var count = await device.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await device.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<DeviceView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<DeviceView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var device = await dbContext.Devices
            .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken)
            ?? throw  new ValidationException("DeviceEntity Not Found");
        
        return device.MapToView();
    }

    #endregion

    #region Mutations

    public async virtual Task Create(CreateDeviceCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        DeviceEntity device = new();
        Reattach(device, command.Entity, dbContext); 
        
        dbContext.Update(device);
        await dbContext.SaveChangesAsync(cancellationToken);

    }

    public async virtual Task Update(UpdateDeviceCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var device = await dbContext.Devices
            .FirstOrDefaultAsync(x => x.Id == command.Entity.Id, cancellationToken)
            ?? throw  new ValidationException("DeviceEntity Not Found");

        Reattach(device, command.Entity, dbContext);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async virtual Task Delete(DeleteDeviceCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var device = await dbContext.Devices
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw  new ValidationException("DeviceEntity Not Found");
        dbContext.Remove(device);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    private static void Reattach(DeviceEntity device, DeviceView deviceView, AppDbContext dbContext)
    {
        DeviceMapper.From(deviceView, device);

    }

    private static void Sorting(ref IQueryable<DeviceEntity> device, TableOptions options) 
        => device = options.SortLabel switch
        {
            "UserId" => device.Ordering(options, o => o.UserId),
            "FirebaseToken" => device.Ordering(options, o => o.FirebaseToken),
            "OsVersion" => device.Ordering(options, o => o.OsVersion),
            "Model" => device.Ordering(options, o => o.Model),
            "AppVersion" => device.Ordering(options, o => o.AppVersion),
            "Session" => device.Ordering(options, o => o.Session),
            "Id" => device.Ordering(options, o => o.Id),
            _ => device.OrderBy(o => o.Id),
        
        };

    #endregion
}

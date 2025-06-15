using myuzbekistan.Services;

public class ServiceTypeService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), IServiceTypeService
{
    #region Queries

    [ComputeMethod]
    public async virtual Task<TableResponse<ServiceTypeView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var servicetype = from s in dbContext.ServiceTypes select s;

        if (!string.IsNullOrEmpty(options.Search))
        {
            servicetype = servicetype.Where(s =>
                     s.Name.Contains(options.Search)
                    || s.Locale.Contains(options.Search)
            );
        }

        #region Search by Language

        if (!String.IsNullOrEmpty(options.Lang))
            servicetype = servicetype.Where(x => x.Locale.Equals(options.Lang));

        #endregion

        Sorting(ref servicetype, options);

        var count = await servicetype.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await servicetype.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<ServiceTypeView>() { Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<List<ServiceTypeView>> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var servicetype = await dbContext.ServiceTypes
            .Where(x => x.Id == Id)
            .ToListAsync(cancellationToken)
            ?? throw new ValidationException("ServiceTypeEntity Not Found");

        return servicetype.MapToViewList();
    }

    #endregion

    #region Mutations
    long maxId;

    public async virtual Task Create(CreateServiceTypeCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        maxId = dbContext.ServiceTypes.Count() == 0 ? 0 : dbContext.ServiceTypes.Max(x => x.Id);
        maxId++;
        foreach (var item in command.Entity)
        {
            ServiceTypeEntity category = new();
            Reattach(category, item, dbContext);
            category.Id = maxId;
            dbContext.Add(category);
        }
        await dbContext.SaveChangesAsync(cancellationToken);

    }

    public async virtual Task Update(UpdateServiceTypeCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var stp = command.Entity.First();

        var serviceType = dbContext.ServiceTypes
        .Where(x => x.Id == stp.Id).ToList() ?? throw new ValidationException("MerchantCategoryEntity Not Found");
        foreach (var item in command.Entity)
        {
            Reattach(serviceType.First(x => x.Locale == item.Locale), item, dbContext);
            dbContext.Update(serviceType.First(x => x.Locale == item.Locale));
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async virtual Task Delete(DeleteServiceTypeCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);

        var serviceType = dbContext.ServiceTypes
      .Where(x => x.Id == command.Id)
      .ToList();
        if (serviceType == null) throw new ValidationException("MerchantCategoryEntity Not Found");
        dbContext.RemoveRange(serviceType);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    private static void Reattach(ServiceTypeEntity servicetype, ServiceTypeView servicetypeView, AppDbContext dbContext)
    {
        ServiceTypeMapper.From(servicetypeView, servicetype);

    }

    private static void Sorting(ref IQueryable<ServiceTypeEntity> servicetype, TableOptions options)
        => servicetype = options.SortLabel switch
        {
            "Name" => servicetype.Ordering(options, o => o.Name),
            "Locale" => servicetype.Ordering(options, o => o.Locale),
            "Id" => servicetype.Ordering(options, o => o.Id),
            _ => servicetype.OrderBy(o => o.Id),

        };

    #endregion
}

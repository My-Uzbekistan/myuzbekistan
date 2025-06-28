using myuzbekistan.Services;

public class SimCountryService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), ISimCountryService 
{
    #region Queries

    [ComputeMethod]
    public async virtual Task<TableResponse<SimCountryView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var simcountry = from s in dbContext.SimCountries select s;

        if (!string.IsNullOrEmpty(options.Search))
        {
            simcountry = simcountry.Where(s => 
                     s.Locale.Contains(options.Search)
                    || s.Name.Contains(options.Search)
                    || s.Title.Contains(options.Search)
                    || s.Code.Contains(options.Search)
            );
        }

        Sorting(ref simcountry, options);
        
        var count = await simcountry.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await simcountry.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<SimCountryView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<SimCountryView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var simcountry = await dbContext.SimCountries
            .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken)
            ?? throw  new ValidationException("SimCountryEntity Not Found");
        
        return simcountry.MapToView();
    }

    #endregion

    #region Mutations

    public async virtual Task Create(CreateSimCountryCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        SimCountryEntity simcountry = new();
        Reattach(simcountry, command.Entity, dbContext); 
        
        dbContext.Update(simcountry);
        await dbContext.SaveChangesAsync(cancellationToken);

    }

    public async virtual Task Update(UpdateSimCountryCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var simcountry = await dbContext.SimCountries
            .FirstOrDefaultAsync(x => x.Id == command.Entity.Id, cancellationToken)
            ?? throw  new ValidationException("SimCountryEntity Not Found");

        Reattach(simcountry, command.Entity, dbContext);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async virtual Task Delete(DeleteSimCountryCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var simcountry = await dbContext.SimCountries
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw  new ValidationException("SimCountryEntity Not Found");
        dbContext.Remove(simcountry);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    private static void Reattach(SimCountryEntity simcountry, SimCountryView simcountryView, AppDbContext dbContext)
    {
        SimCountryMapper.From(simcountryView, simcountry);

    }

    private static void Sorting(ref IQueryable<SimCountryEntity> simcountry, TableOptions options) 
        => simcountry = options.SortLabel switch
        {
            "Locale" => simcountry.Ordering(options, o => o.Locale),
            "Name" => simcountry.Ordering(options, o => o.Name),
            "Title" => simcountry.Ordering(options, o => o.Title),
            "Code" => simcountry.Ordering(options, o => o.Code),
            "Status" => simcountry.Ordering(options, o => o.Status),
            "Id" => simcountry.Ordering(options, o => o.Id),
            _ => simcountry.OrderBy(o => o.Id),
        
        };

    #endregion
}

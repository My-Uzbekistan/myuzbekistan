using Microsoft.EntityFrameworkCore;
using ActualLab.Fusion;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using ActualLab.Async;
using System.Reactive;
namespace myuzbekistan.Services;

public class FacilityService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), IFacilityService 
{
    #region Queries
    [ComputeMethod]
    public async virtual Task<TableResponse<FacilityView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var facility = from s in dbContext.Facilities select s;

        if (!String.IsNullOrEmpty(options.Search))
        {
            facility = facility.Where(s => 
                     s.Name.Contains(options.Search)
                    || s.Locale.Contains(options.Search)
            );
        }

        Sorting(ref facility, options);

        if (!String.IsNullOrEmpty(options.Lang))
            facility = facility.Where(x => x.Locale.Equals(options.Lang));

        facility = facility.Include(x => x.Icon);
        var count = await facility.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await facility.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<FacilityView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<List<FacilityView>> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var facility = dbContext.Facilities
        .Include(x => x.Icon)
        .Where(x => x.Id == Id)
        .ToList();
        
        return facility == null ? throw new ValidationException("FacilityEntity Not Found") : facility.MapToViewList();
    }

    #endregion
    #region Mutations
    long maxId;
    public async virtual Task Create(CreateFacilityCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        maxId = !dbContext.Facilities.Any() ? 0 : dbContext.Facilities.Max(x => x.Id);
        maxId++;
        foreach (var item in command.Entity)
        {
            FacilityEntity facility = new FacilityEntity();
            Reattach(facility, item, dbContext);
            facility.Id = maxId;
            dbContext.Add(facility);

        }
        
        await dbContext.SaveChangesAsync(cancellationToken);

    }


    public async virtual Task Delete(DeleteFacilityCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var facility = await dbContext.Facilities
        .Include(x=>x.Icon)
        .FirstOrDefaultAsync(x => x.Id == command.Id);
        if (facility == null) throw  new ValidationException("FacilityEntity Not Found");
        dbContext.Remove(facility);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task Update(UpdateFacilityCommand command, CancellationToken cancellationToken = default)
    {
        var fac = command.Entity.First();
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var facility = dbContext.Facilities
            .Include(x=>x.Icon)
        .Where(x => x.Id == fac.Id).AsNoTracking().ToList();

        if (facility == null) throw new ValidationException("FacilityEntity Not Found");

        foreach (var item in command.Entity)
        {
            Reattach(facility.First(x => x.Locale == item.Locale), item, dbContext);
            dbContext.Update(facility.First(x => x.Locale == item.Locale));
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
    private void Reattach(FacilityEntity facility, FacilityView facilityView, AppDbContext dbContext)
    {
        FacilityMapper.From(facilityView, facility);


        if(facility.Icon != null)
        facility.Icon = dbContext.Files
        .First(x => x.Id == facility.Icon.Id);

    }

    private void Sorting(ref IQueryable<FacilityEntity> facility, TableOptions options) => facility = options.SortLabel switch
    {
        "Name" => facility.Ordering(options, o => o.Name),
        "Locale" => facility.Ordering(options, o => o.Locale),
        "Icon" => facility.Ordering(options, o => o.Icon),
        "Id" => facility.Ordering(options, o => o.Id),
        _ => facility.OrderBy(o => o.Id),
        
    };
    #endregion
}

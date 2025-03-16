using Microsoft.EntityFrameworkCore;
using ActualLab.Fusion;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using ActualLab.Async;
using System.Reactive;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Internal;
using ActualLab.Api;
namespace myuzbekistan.Services;

public class RegionService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), IRegionService 
{
    #region Queries
    [ComputeMethod]
    public async virtual Task<TableResponse<RegionView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var region = (from s in dbContext.Regions
                      join r in dbContext.Regions
                          on new { ParentRegionId = s.ParentRegionId ?? 0, s.Locale } equals new { ParentRegionId = r.Id, r.Locale }
                      where s.Locale == CultureInfo.CurrentCulture.TwoLetterISOLanguageName
                      select new RegionEntity
                      {
                          Id = s.Id,
                          Name = s.Name,
                          Locale = s.Locale,
                          ParentRegionId = s.ParentRegionId,
                          ParentRegion = r
                      });
        
        if (!String.IsNullOrEmpty(options.Search))
        {
            region = region.Where(s => 
                     s.Name.Contains(options.Search)
                    || s.Locale.Contains(options.Search)
            );
        }

        if (!String.IsNullOrEmpty(options.Lang)){
            region = region.Where(s => s.Locale == options.Lang);
        }

        Sorting(ref region, options);
        

       
        var count = await region.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await region.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<RegionView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    public async virtual Task<List<RegionApi>> GetRegions(CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var region = (from s in dbContext.Regions
                      //join r in dbContext.Regions
                      //    on new { ParentRegionId = s.ParentRegionId ?? 0, s.Locale } equals new { ParentRegionId = r.Id, r.Locale }
                      where s.Locale == CultureInfo.CurrentCulture.TwoLetterISOLanguageName
                      select new RegionEntity
                      {
                          Id = s.Id,
                          Name = s.Name,
                          Locale = s.Locale,
                          ParentRegionId = s.ParentRegionId,
                          //ParentRegion = r
                      });

       
        return region.ToList().MapToApiList();
    }

    [ComputeMethod]
    public async virtual Task<List<RegionView>> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var region = await dbContext.Regions
        .Include(x => x.ParentRegion)
        .Where(x => x.Id == Id).ToListAsync(cancellationToken: cancellationToken);
        
        return region == null ? throw new ValidationException("RegionEntity Not Found") : region.MapToViewList();
    }

    #endregion
    #region Mutations
    long maxId;
    public async virtual Task Create(CreateRegionCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        maxId = dbContext.Regions.Count() == 0 ? 0 : dbContext.Categories.Max(x => x.Id);
        maxId++;
        foreach (var item in command.Entity)
        {
            RegionEntity region = new();
            Reattach(region, item, dbContext);
            region.Id = maxId;
            dbContext.Add(region);

        }
        await dbContext.SaveChangesAsync(cancellationToken);

    }


    public async virtual Task Delete(DeleteRegionCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var region =  dbContext.Regions
        .Include(x=>x.ParentRegion)
        .Where(x => x.Id == command.Id)
        .ToList();

        if (region == null) throw  new ValidationException("RegionEntity Not Found");
        dbContext.RemoveRange(region);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task Update(UpdateRegionCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var regions = dbContext.Regions
        .Include(x => x.Contents);

        if (regions == null) throw new ValidationException("RegionEntity Not Found");

        foreach (var item in command.Entity)
        {
            Reattach(regions.First(x => x.Locale == item.Locale), item, dbContext);
            dbContext.Update(regions.First(x => x.Locale == item.Locale));
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
    private void Reattach(RegionEntity region, RegionView regionView, AppDbContext dbContext)
    {
        RegionMapper.From(regionView, region);


        if(region.ParentRegion != null)
        region.ParentRegion = dbContext.Regions
        .First(x => x.Id == region.ParentRegion.Id);

    }

    private void Sorting(ref IQueryable<RegionEntity> region, TableOptions options) => region = options.SortLabel switch
    {
        "Name" => region.Ordering(options, o => o.Name),
        "Locale" => region.Ordering(options, o => o.Locale),
        "ParentRegion" => region.Ordering(options, o => o.ParentRegion),
        "Id" => region.Ordering(options, o => o.Id),
        _ => region.OrderBy(o => o.Id),
        
    };
    #endregion
}

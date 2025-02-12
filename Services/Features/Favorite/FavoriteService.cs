using Microsoft.EntityFrameworkCore;
using ActualLab.Fusion;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using ActualLab.Async;
using System.Reactive;
namespace myuzbekistan.Services;

public class FavoriteService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), IFavoriteService 
{
    #region Queries
    [ComputeMethod]
    public async virtual Task<TableResponse<FavoriteView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var favorite = from s in dbContext.Favorites select s;

        if (!String.IsNullOrEmpty(options.Search))
        {
            favorite = favorite.Where(s => 
                s.Id.ToString().Contains(options.Search)
            );
        }

        Sorting(ref favorite, options);
        
        favorite = favorite.Include(x => x.Content);
        var count = await favorite.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await favorite.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<FavoriteView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<FavoriteView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var favorite = await dbContext.Favorites
        .Include(x => x.Content)
        .FirstOrDefaultAsync(x => x.Id == Id);
        
        return favorite == null ? throw new ValidationException("FavoriteEntity Not Found") : favorite.MapToView();
    }

    #endregion
    #region Mutations
    public async virtual Task Create(CreateFavoriteCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        FavoriteEntity favorite=new FavoriteEntity();
        Reattach(favorite, command.Entity, dbContext); 
        
        dbContext.Update(favorite);
        await dbContext.SaveChangesAsync(cancellationToken);

    }


    public async virtual Task Delete(DeleteFavoriteCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var favorite = await dbContext.Favorites
        .Include(x=>x.Content)
        .FirstOrDefaultAsync(x => x.Id == command.Id);
        if (favorite == null) throw  new ValidationException("FavoriteEntity Not Found");
        dbContext.Remove(favorite);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task Update(UpdateFavoriteCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var favorite = await dbContext.Favorites
        .Include(x=>x.Content)
        .FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

        if (favorite == null) throw  new ValidationException("FavoriteEntity Not Found"); 

        Reattach(favorite, command.Entity, dbContext);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
    private void Reattach(FavoriteEntity favorite, FavoriteView favoriteView, AppDbContext dbContext)
    {
        FavoriteMapper.From(favoriteView, favorite);


        if(favorite.Content != null)
        favorite.Content = dbContext.Contents
        .First(x => x.Id == favorite.Content.Id);

    }

    private void Sorting(ref IQueryable<FavoriteEntity> favorite, TableOptions options) => favorite = options.SortLabel switch
    {
        "Content" => favorite.Ordering(options, o => o.Content),
        "Id" => favorite.Ordering(options, o => o.Id),
        _ => favorite.OrderBy(o => o.Id),
        
    };
    #endregion
}

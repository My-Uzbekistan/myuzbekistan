using Microsoft.EntityFrameworkCore;
using ActualLab.Fusion;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using ActualLab.Async;
using System.Reactive;
using static System.Collections.Specialized.BitVector32;
namespace myuzbekistan.Services;

public class CategoryService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), ICategoryService 
{
    #region Queries
    [ComputeMethod]
    public async virtual Task<TableResponse<CategoryView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var category = from s in dbContext.Categories select s;

        if (!String.IsNullOrEmpty(options.Search))
        {
            category = category.Where(s => 
                     s.Name.Contains(options.Search)
                    || s.Description.Contains(options.Search)
            );
        }

        #region Search by Language

        if (!String.IsNullOrEmpty(options.Lang))
            category = category.Where(x => x.Locale.Equals(options.Lang));

        #endregion

        Sorting(ref category, options);
        
        category = category.Include(x => x.Contents);
        var count = await category.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await category.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<CategoryView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<List<CategoryView>> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var category = dbContext.Categories
        .Include(x => x.Contents)
        .Where(x => x.Id == Id).ToList();
        
        return category == null ? throw new ValidationException("CategoryEntity Not Found") : category.MapToViewList();
    }

    #endregion
    #region Mutations
    long maxId;
    public async virtual Task Create(CreateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        maxId = dbContext.Categories.Count() == 0 ? 0 : dbContext.Categories.Max(x=>x.Id);
        maxId++;
        foreach (var item in command.Entity)
        {
            CategoryEntity category = new CategoryEntity();
            Reattach(category, item, dbContext);
            category.Id = maxId;
            dbContext.Add(category);

        }
        
        await dbContext.SaveChangesAsync(cancellationToken);

    }


    public async virtual Task Delete(DeleteCategoryCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var category = await dbContext.Categories
        .Include(x=>x.Contents)
        .FirstOrDefaultAsync(x => x.Id == command.Id);
        if (category == null) throw  new ValidationException("CategoryEntity Not Found");
        dbContext.Remove(category);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task Update(UpdateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        var cat = command.Entity.First();
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var category = dbContext.Categories
        .Include(x=>x.Contents)
        .Where(x => x.Id == cat.Id).AsNoTracking().ToList();

        if (category == null) throw new ValidationException("CategoryEntity Not Found");

        foreach (var item in command.Entity)
        {
            Reattach(category.First(x=>x.Locale == item.Locale), item, dbContext);
            dbContext.Update(category.First(x=>x.Locale == item.Locale));
        }

        
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
    private void Reattach(CategoryEntity category, CategoryView categoryView, AppDbContext dbContext)
    {
        CategoryMapper.From(categoryView, category);


        if(category.Contents != null)
        category.Contents  = dbContext.Contents
        .Where(x => category.Contents.Select(tt => tt.Id).ToList().Contains(x.Id)).ToList();

    }

    private void Sorting(ref IQueryable<CategoryEntity> category, TableOptions options) => category = options.SortLabel switch
    {
        "Name" => category.Ordering(options, o => o.Name),
        "Description" => category.Ordering(options, o => o.Description),
        "Contents" => category.Ordering(options, o => o.Contents),
        "Id" => category.Ordering(options, o => o.Id),
        _ => category.OrderBy(o => o.Id),
        
    };
    #endregion
}

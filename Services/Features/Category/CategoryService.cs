using Microsoft.EntityFrameworkCore;
using ActualLab.Fusion;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using ActualLab.Async;
using System.Reactive;
using System.Globalization;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace myuzbekistan.Services;

public class CategoryService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), ICategoryService
{
    #region Queries
    public async Task<List<MainPageApi>> GetMainPageApi(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var query = dbContext.Categories
            .Where(c => c.Status == ContentStatus.Active &&
                        c.Locale == CultureInfo.CurrentCulture.TwoLetterISOLanguageName)

            .Include(c => c.Icon)
            .Include(c => c.Contents!)
                .ThenInclude(content => content.Reviews)
            .Include(c => c.Contents!)
                .ThenInclude(content => content.Photos)
                .Include(c => c.Contents!)
                .ThenInclude(content => content.Photo)
            .Include(c => c.Contents!)
                .ThenInclude(content => content.Languages)
            .Include(c => c.Contents!)
                .ThenInclude(content => content.Files)
            .Include(c => c.Contents!)
                .ThenInclude(content => content.Region)
            .Include(c => c.Contents!)
                .ThenInclude(content => content.Facilities!)
                    .ThenInclude(f => f.Icon).AsQueryable();

        if (options.IsMore.HasValue && options.IsMore.Value)
        {
            query = query.Where(x => x.ViewType == ViewType.More);
        }
        else
        {
            query = query.Where(x => x.ViewType != ViewType.More);
        }

        var catQuery = query
            .AsAsyncEnumerable()
            .OrderBy(x => x.Order)
            .Select(c => new MainPageApi(
                c.Name,
                c.Id,
                c.Contents!
                    .Where(x =>
                        (options.RegionId == null || options.RegionId == 1)
                            ? x.GlobalRecommended
                            : x.Recommended && x.RegionId == options.RegionId)
                    .FirstOrDefault()
                    ?.MapToApi(),
                c.ViewType,
                [.. ContentQuery(c.Contents!,options).OrderBy(_ => Guid.NewGuid())
                .Select(x => x.MapToApi())]))
            .Where(s =>
                string.IsNullOrEmpty(options.Search) ||
                s.CategoryName.ToLower().Contains(options.Search.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                s.Contents.Any(t => t.Region.ToLower().Contains(options.Search.ToLower(), StringComparison.OrdinalIgnoreCase))
            );

        var categories = await catQuery.ToListAsync(cancellationToken);

        return [.. categories];
    }

    private IEnumerable<ContentEntity> ContentQuery(List<ContentEntity> contentEntities, TableOptions options)
    {
        if (options.RegionId is null || options.RegionId == 1)
        {

            return contentEntities.Where(content =>
                               string.IsNullOrEmpty(options.Search) ||
                               content.Title.ToLower().Contains(options.Search.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                               (content.Address != null && content.Address.ToLower().Contains(options.Search.ToLower(), StringComparison.OrdinalIgnoreCase)))
                               .Where(content => content.Status == ContentStatus.Active);
        }

        return contentEntities.Where(content =>
                            string.IsNullOrEmpty(options.Search) ||
                            content.Title.ToLower().Contains(options.Search.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                            (content.Address != null && content.Address.ToLower().Contains(options.Search.ToLower(), StringComparison.OrdinalIgnoreCase)))
                            .Where(content =>
                                options.RegionId == null ||
                                (
                                    content.Region != null &&
                                    content.Region.IsActive && content.RegionId == options.RegionId &&
                                    content.Status == ContentStatus.Active
                                )
                            );

    }

    //[ComputeMethod]
    public async virtual Task<List<CategoryApi>> GetCategories(CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var category = from s in dbContext.Categories.Include(x => x.Icon)
                       where s.Status == ContentStatus.Active && s.Locale == CultureInfo.CurrentCulture.TwoLetterISOLanguageName && s.ViewType != ViewType.More
                       orderby s.Order
                       select new CategoryApi(s.Name, s.Icon!.Path!, s.Id);

        return [.. category];
    }

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

        if (options.IsMore.HasValue && options.IsMore.Value)
        {
            category = category.Where(x => x.ViewType == ViewType.More);
        }
        else
        {
            category = category.Where(x => x.ViewType != ViewType.More);
        }
        if (!options.WithoutExpand)
        {
            category = category.Include(x => x.Contents).Include(x => x.Icon);
        }

        var count = await category.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await category.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<CategoryView>() { Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<List<CategoryView>> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var category = dbContext.Categories
        .Include(x => x.Contents)
        .Include(x => x.Icon)
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
        maxId = dbContext.Categories.Count() == 0 ? 0 : dbContext.Categories.Max(x => x.Id);
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
        var category = dbContext.Categories
        .Include(x => x.Contents)
        .Include(x => x.Icon)
        .Where(x => x.Id == command.Id)
        .ToList();
        if (category == null) throw new ValidationException("CategoryEntity Not Found");
        dbContext.RemoveRange(category);
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
        .Include(x => x.Contents)
        .Include(x => x.Icon)
        .Where(x => x.Id == cat.Id).ToList();

        if (category == null) throw new ValidationException("CategoryEntity Not Found");

        foreach (var item in command.Entity)
        {
            Reattach(category.First(x => x.Locale == item.Locale), item, dbContext);
            dbContext.Update(category.First(x => x.Locale == item.Locale));
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


        if (category.Contents != null)
            category.Contents = dbContext.Contents.Where(x => x.CategoryId == category.Id && x.Locale == categoryView.Locale).ToList();

        if (categoryView.IconView != null || category.Icon?.Path != categoryView.IconView?.Path)
            category.Icon = dbContext.Files
            .Where(x => x.Path == categoryView.IconView!.Path).First();

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

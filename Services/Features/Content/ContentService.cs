using Microsoft.EntityFrameworkCore;
using ActualLab.Fusion;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using ActualLab.Async;
using System.Reactive;
namespace myuzbekistan.Services;

public class ContentService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), IContentService
{
    #region Queries
    [ComputeMethod]
    public async virtual Task<TableResponse<ContentView>> GetAll(long CategoryId, TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var content = from s in dbContext.Contents select s;

        if (!String.IsNullOrEmpty(options.Search))
        {
            content = content.Where(s =>
                     s.Title.Contains(options.Search)
                    || s.Description.Contains(options.Search)
                    || s.WorkingHours.Contains(options.Search)
                    || (s.Facilities != null && s.Facilities.Any(x => x.Name.Contains(options.Search)))
                    || (s.Languages != null && s.Languages.Any(x => x.Name.Contains(options.Search)))
                    || s.Address != null && s.Address.Contains(options.Search)
            );
        }
        #region CategoryId

        content = content.Where(x => x.CategoryId == CategoryId);
        #endregion

        Sorting(ref content, options);

        content = content.Include(x => x.Category);
        content = content.Include(x => x.Files);
        content = content.Include(x => x.Photos);
        content = content.Include(x => x.Reviews);
        content = content.Include(x => x.Facilities);
        content = content.Include(x => x.Languages);
        var count = await content.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await content.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<ContentView>() { Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<List<ContentView>> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var content = dbContext.Contents
        .Include(x => x.Category)
        .Include(x => x.Files)
        .Include(x => x.Photos)
        .Include(x => x.Reviews)
        .Where(x => x.Id == Id).ToList();

        return content == null ? throw new ValidationException("ContentEntity Not Found") : content.MapToViewList();
    }

    #endregion
    #region Mutations
    long maxId;
    public async virtual Task Create(CreateContentCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        maxId = !dbContext.Categories.Any() ? 0 : dbContext.Categories.Max(x => x.Id);
        maxId++;
        foreach (var item in command.Entity)
        {
            ContentEntity content = new ContentEntity();
            Reattach(content, item, dbContext);
            content.Id = maxId;
            dbContext.Add(content);

        }


        await dbContext.SaveChangesAsync(cancellationToken);

    }


    public async virtual Task Delete(DeleteContentCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var content = await dbContext.Contents
        .Include(x => x.Category)
        .Include(x => x.Files)
        .Include(x => x.Photos)
        .Include(x => x.Reviews)
        .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken) ?? throw new ValidationException("ContentEntity Not Found");
        dbContext.Remove(content);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task Update(UpdateContentCommand command, CancellationToken cancellationToken = default)
    {
        var con = command.Entity.First();
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var content = dbContext.Contents
        .Include(x => x.Category)
        .Include(x => x.Files)
        .Include(x => x.Photos)
        .Include(x => x.Reviews)
        .Where(x => x.Id == con.Id).ToList() ?? throw new ValidationException("ContentEntity Not Found");

        foreach (var item in command.Entity)
        {
            Reattach(content.First(x => x.Locale == item.Locale), item, dbContext);
            dbContext.Update(content.First(x => x.Locale == item.Locale));
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion



    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
    private void Reattach(ContentEntity content, ContentView contentView, AppDbContext dbContext)
    {
        ContentMapper.From(contentView, content);


        if (content.Category != null)
            content.Category = dbContext.Categories
            .First(x => x.Id == content.Category.Id);
        if (content.Files != null)
            content.Files = [.. dbContext.Files.Where(x => content.Files.Select(tt => tt.Id).ToList().Contains(x.Id))];
        if (content.Photos != null)
            content.Photos = [.. dbContext.Files.Where(x => content.Photos.Select(tt => tt.Id).ToList().Contains(x.Id))];
        if (content.Reviews != null)
            content.Reviews = [.. dbContext.Reviews.Where(x => content.Reviews.Select(tt => tt.Id).ToList().Contains(x.Id))];

    }

    private void Sorting(ref IQueryable<ContentEntity> content, TableOptions options) => content = options.SortLabel switch
    {
        "Title" => content.Ordering(options, o => o.Title),
        "Description" => content.Ordering(options, o => o.Description),
        "CategoryId" => content.Ordering(options, o => o.CategoryId),
        "Category" => content.Ordering(options, o => o.Category),
        "WorkingHours" => content.Ordering(options, o => o.WorkingHours),
        "Facilities" => content.Ordering(options, o => o.Facilities),
        "Location" => content.Ordering(options, o => o.Location),
        "PhoneNumbers" => content.Ordering(options, o => o.PhoneNumbers),
        "Files" => content.Ordering(options, o => o.Files),
        "Photos" => content.Ordering(options, o => o.Photos),
        "Reviews" => content.Ordering(options, o => o.Reviews),
        "Languages" => content.Ordering(options, o => o.Languages),
        "RatingAverage" => content.Ordering(options, o => o.RatingAverage),
        "Price" => content.Ordering(options, o => o.Price),
        "PriceInDollar" => content.Ordering(options, o => o.PriceInDollar),
        "Address" => content.Ordering(options, o => o.Address),
        "Recommended" => content.Ordering(options, o => o.Recommended),
        "Id" => content.Ordering(options, o => o.Id),
        _ => content.OrderBy(o => o.Id),

    };
    #endregion
}

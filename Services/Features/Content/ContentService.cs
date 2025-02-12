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
    public async virtual Task<TableResponse<ContentView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var content = from s in dbContext.Contents select s;

        if (!String.IsNullOrEmpty(options.Search))
        {
            content = content.Where(s => 
                     s.Title.Contains(options.Search)
                    || s.Description.Contains(options.Search)
                    || s.CategoryId.Contains(options.Search)
                    || s.Slug.Contains(options.Search)
                    || s.WorkingHours.Contains(options.Search)
                    || s.Facilities.Contains(options.Search)
                    || s.Languages.Contains(options.Search)
                    || s.Address !=null && s.Address.Contains(options.Search)
            );
        }

        Sorting(ref content, options);
        
        content = content.Include(x => x.Category);
        content = content.Include(x => x.Files);
        content = content.Include(x => x.Photos);
        content = content.Include(x => x.Reviews);
        var count = await content.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await content.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<ContentView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<ContentView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var content = await dbContext.Contents
        .Include(x => x.Category)
        .Include(x => x.Facilities)
        .Include(x => x.PhoneNumbers)
        .Include(x => x.Files)
        .Include(x => x.Photos)
        .Include(x => x.Reviews)
        .Include(x => x.Languages)
        .FirstOrDefaultAsync(x => x.Id == Id);
        
        return content == null ? throw new ValidationException("ContentEntity Not Found") : content.MapToView();
    }

    #endregion
    #region Mutations
    public async virtual Task Create(CreateContentCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        ContentEntity content=new ContentEntity();
        Reattach(content, command.Entity, dbContext); 
        
        dbContext.Update(content);
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
        .Include(x=>x.Category)
        .Include(x=>x.Facilities)
        .Include(x=>x.PhoneNumbers)
        .Include(x=>x.Files)
        .Include(x=>x.Photos)
        .Include(x=>x.Reviews)
        .Include(x=>x.Languages)
        .FirstOrDefaultAsync(x => x.Id == command.Id);
        if (content == null) throw  new ValidationException("ContentEntity Not Found");
        dbContext.Remove(content);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task Update(UpdateContentCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var content = await dbContext.Contents
        .Include(x=>x.Category)
        .Include(x=>x.Facilities)
        .Include(x=>x.PhoneNumbers)
        .Include(x=>x.Files)
        .Include(x=>x.Photos)
        .Include(x=>x.Reviews)
        .Include(x=>x.Languages)
        .FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

        if (content == null) throw  new ValidationException("ContentEntity Not Found"); 

        Reattach(content, command.Entity, dbContext);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
    private void Reattach(ContentEntity content, ContentView contentView, AppDbContext dbContext)
    {
        ContentMapper.From(contentView, content);


        if(content.Category != null)
        content.Category = dbContext.Categories
        .First(x => x.Id == content.Category.Id);
        if(content.Files != null)
        content.Files  = dbContext.Files
        .Where(x => content.Files.Select(tt => tt.Id).ToList().Contains(x.Id)).ToList();
        if(content.Photos != null)
        content.Photos  = dbContext.Files
        .Where(x => content.Photos.Select(tt => tt.Id).ToList().Contains(x.Id)).ToList();
        if(content.Reviews != null)
        content.Reviews  = dbContext.Reviews
        .Where(x => content.Reviews.Select(tt => tt.Id).ToList().Contains(x.Id)).ToList();

    }

    private void Sorting(ref IQueryable<ContentEntity> content, TableOptions options) => content = options.SortLabel switch
    {
        "Title" => content.Ordering(options, o => o.Title),
        "Description" => content.Ordering(options, o => o.Description),
        "CategoryId" => content.Ordering(options, o => o.CategoryId),
        "Category" => content.Ordering(options, o => o.Category),
        "IsPublished" => content.Ordering(options, o => o.IsPublished),
        "IsDeleted" => content.Ordering(options, o => o.IsDeleted),
        "Slug" => content.Ordering(options, o => o.Slug),
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

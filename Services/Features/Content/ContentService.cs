using ActualLab.Async;
using ActualLab.Fusion;
using ActualLab.Fusion.EntityFramework;
using Microsoft.EntityFrameworkCore;
using myuzbekistan.Shared;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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


        if (!String.IsNullOrEmpty(options.Lang))
            content = content.Where(x => x.Locale.Equals(options.Lang));

        Sorting(ref content, options);

        content = content.Include(x => x.Category);
        content = content.Include(x => x.Files);
        content = content.Include(x => x.Photos);
        content = content.Include(x => x.Photo);
        content = content.Include(x => x.Reviews);
        content = content.Include(x => x.Facilities);
        content = content.Include(x => x.Languages);
        var count = await content.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await content.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<ContentView>() { Items = items.MapToViewList(), TotalItems = count };
    }


    [ComputeMethod]
    public async virtual Task<List<ContentApiView>> GetContents(long CategoryId, TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);

        // Начальный запрос
        var content = dbContext.Contents.AsQueryable();

        // Фильтрация по поисковому запросу
        if (!string.IsNullOrEmpty(options.Search))
        {
            var search = options.Search.ToLower();

            content = content.Where(s =>
                s.Title.ToLower().Contains(search) ||
                s.Description.ToLower().Contains(search) ||
                s.WorkingHours.ToLower().Contains(search) ||
                s.Facilities.Any(f => f.Name.ToLower().Contains(search)) ||
                s.Languages.Any(l => l.Name.ToLower().Contains(search)) ||
                s.Address.ToLower().Contains(search)
            );
        }

        // Фильтрация по категории
        content = content.Where(x => x.CategoryId == CategoryId);

        // Фильтрация по языку
        content = content.Where(x => x.Locale.Equals(CultureInfo.CurrentCulture.TwoLetterISOLanguageName));

        // Применение сортировки
        Sorting(ref content, options);

        // Включаем связанные сущности с AsSplitQuery для оптимизации
        content = content
            .Include(x => x.Category)
            .Include(x => x.Files)
            .Include(x => x.Photos)
            .Include(x => x.Photo)
            .Include(x => x.Reviews)
            .Include(x => x.Facilities!).ThenInclude(x => x.Icon)
            .Include(x => x.Languages)
            .AsSplitQuery() // Оптимизация запросов
            .AsNoTracking();

        // Пагинация и выполнение запроса
        var items = await content.Paginate(options).ToListAsync(cancellationToken: cancellationToken);

        // Маппинг данных в API-модель
        var data = items.Select(x => x.MapToApi()).ToList();

        return data;
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
        .Include(x => x.Photo)
        .Include(x => x.Reviews)
        .Include(x => x.Languages)
        .Include(x => x.Facilities!).ThenInclude(x => x.Icon)
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
        maxId = !dbContext.Contents.Any() ? 0 : dbContext.Contents.Max(x => x.Id);
        maxId++;
        foreach (var item in command.Entity)
        {
            ContentEntity content = new ContentEntity();
            Reattach(content, item, dbContext);

            content.Id = maxId;
            dbContext.Add(content);

        }
        var fContent = command.Entity.First();
        if (command.Entity.First().Recommended)
        {
            dbContext.Database.ExecuteSqlInterpolated($"UPDATE  \"Contents\" set \"Recommended\" = false where \"CategoryId\" = {fContent.CategoryId};");
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
        var contents = dbContext.Contents
            .Include(x => x.Category)
        .Include(x => x.Files)
        .Include(x => x.Photos)
        .Include(x => x.Photo)
        .Include(x => x.Reviews)
        .Include(x => x.Languages)
        .Include(x => x.Facilities)
        .Include(x => x.Reviews)
        .Where(x => x.Id == command.Id).ToList();
        if (contents.Count == 0)
        {
            throw new ValidationException("ContentEntity Not Found");
        }

        dbContext.RemoveRange(contents);
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
        .Include(x => x.Photo)
        .Include(x => x.Reviews)
        .Include(x => x.Languages)
        .Include(x => x.Facilities)
        .Where(x => x.Id == con.Id).ToList() ?? throw new ValidationException("ContentEntity Not Found");

        foreach (var item in command.Entity)
        {
            var cont = content.First(x => x.Locale == item.Locale);
            Reattach(cont, item, dbContext);
            dbContext.Entry(cont).State = EntityState.Modified;
        }



        await dbContext.SaveChangesAsync(cancellationToken);
        if (con.PhotoView != null)
            dbContext.Database.ExecuteSqlInterpolated($"UPDATE  \"Contents\" set \"PhotoId\" = {con.PhotoView.Id} where \"Id\" = {con.Id};");
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
            .First(x => x.Id == content.Category.Id && x.Locale == content.Category.Locale);
        if (content.Files != null)
            content.Files = [.. dbContext.Files.Where(x => content.Files.Select(tt => tt.Id).ToList().Contains(x.Id))];
        var photo = contentView.PhotoView == null ? null : dbContext.Files.First(x => x.Id == contentView.PhotoView.Id);
        content.Photo = photo;
        if (content.Photos != null)
            content.Photos = [.. dbContext.Files.Where(x => content.Photos.Select(tt => tt.Id).ToList().Contains(x.Id))];
        if (content.Reviews != null)
            content.Reviews = [.. dbContext.Reviews.Where(x => content.Reviews.Select(tt => tt.Id).ToList().Contains(x.Id))];
        if (content.Languages != null)
            content.Languages = [.. dbContext.Languages.Where(x => content.Languages.Select(tt => tt.Id).ToList().Contains(x.Id) && x.Locale == contentView.Locale)];
        if (content.Facilities != null)
            content.Facilities = [.. dbContext.Facilities.Where(x => content.Facilities.Select(tt => tt.Id).ToList().Contains(x.Id) && x.Locale == contentView.Locale)];

    }

    private void Sorting(ref IQueryable<ContentEntity> content, TableOptions options) => content = options.SortLabel switch
    {
        "Title" => content.Ordering(options, o => o.Title),
        "Description" => content.Ordering(options, o => o.Description),
        "CategoryId" => content.Ordering(options, o => o.CategoryId),
        "Category" => content.Ordering(options, o => o.Category),
        "WorkingHours" => content.Ordering(options, o => o.WorkingHours),
        "Location" => content.Ordering(options, o => o.Location),
        "Files" => content.Ordering(options, o => o.Files),
        "Photos" => content.Ordering(options, o => o.Photos),
        "Reviews" => content.Ordering(options, o => o.Reviews),
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

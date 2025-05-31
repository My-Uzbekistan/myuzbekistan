using ActualLab.Async;
using ActualLab.Fusion;
using ActualLab.Fusion.Authentication;
using ActualLab.Fusion.EntityFramework;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using myuzbekistan.Shared;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reactive;
using System.Security.Claims;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
namespace myuzbekistan.Services;

public class ContentService(IServiceProvider services, ICategoryService categoryService, ILogger<ContentService> logger, IAuth auth) : DbServiceBase<AppDbContext>(services), IContentService
{
    #region Queries

    public async virtual Task<List<ContentShort>> GetContentByCategoryName(string CategoryName, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var content = from s in dbContext.Contents select s;

        var cnt = dbContext.Contents.FirstOrDefault(x => x.Category.Name.Contains(CategoryName));
        if (cnt == null)
        {
            return [];
        }
        #region CategoryId
        content = content.Where(x => x.CategoryId == cnt.CategoryId);
        #endregion

        content = content.Where(x => x.Status == ContentStatus.Active);

        content = content.Where(x => x.Locale.Equals(CultureInfo.CurrentCulture.TwoLetterISOLanguageName));


        content = content.Include(x => x.Category);
        content = content.Include(x => x.Files);
        content = content.Include(x => x.Photos);
        content = content.Include(x => x.Photo);
        content = content.Include(x => x.Reviews);
        content = content.Include(x => x.Facilities);
        content = content.Include(x => x.Languages);
        content = content.Include(x => x.Region);

        logger.LogError(content.ToQueryString());

        var items = await content.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
        return [.. items.Select(x => x.MapToShortApi())];
    }

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

        if (options.RegionId != null && options.RegionId != 0)
        {
            content = content.Where(x => x.RegionId == options.RegionId);
        }

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
        content = content.Include(x => x.Region);

        var count = await content.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await content.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<ContentView>() { Items = items.MapToViewList(), TotalItems = count };
    }




    [ComputeMethod]
    public async virtual Task<List<MainPageContent>> GetContents(long CategoryId, long userId, TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);

        var content = dbContext.Contents.AsQueryable();

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

        content = content.Where(x => x.CategoryId == CategoryId);

        content = content.Where(x => x.Locale.Equals(CultureInfo.CurrentCulture.TwoLetterISOLanguageName));

        content = content.Where(x => x.Status == ContentStatus.Active);

        Sorting(ref content, options);

        var query = content
            .Include(x => x.Category)
            .Include(x => x.Files)
            .Include(x => x.Photos)
            .Include(x => x.Photo)
            .Include(x => x.Reviews)
            .Include(x => x.Region)
            .Include(x => x.Facilities!).ThenInclude(x => x.Icon)
            .Include(x => x.Languages)
            .AsSplitQuery()
            .AsNoTracking()
                .Select(x => mapToMainPage(x,dbContext, userId));
        ;

        var items = await query.Paginate(options).ToListAsync(cancellationToken: cancellationToken);

        return items;
    }



    [ComputeMethod]
    public async virtual Task<List<MainPageContent>> GetContentsByIds(List<long> contentIds, long userId, TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);

        var content = dbContext.Contents.AsQueryable();

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

        content = content.Where(x => contentIds.Contains(x.Id));

        content = content.Where(x => x.Locale.Equals(CultureInfo.CurrentCulture.TwoLetterISOLanguageName));
        content = content.Where(x => x.Status == ContentStatus.Active);

        Sorting(ref content, options);

        var query = content
            .Include(x => x.Category)
            .Include(x => x.Files)
            .Include(x => x.Photos)
            .Include(x => x.Photo)
            .Include(x => x.Reviews)
            .Include(x => x.Region)
            .Include(x => x.Facilities!).ThenInclude(x => x.Icon)
            .Include(x => x.Languages)
            .AsSplitQuery()
            .AsNoTracking()
            .Select(x => mapToMainPage(x,dbContext, userId));
        ;

        return await query.Paginate(options).ToListAsync(cancellationToken: cancellationToken);
    }


    private static MainPageContent mapToMainPage(ContentEntity x, AppDbContext dbContext, long userId)
    {
        return new MainPageContent
        {
            ContentId = x.Id,
            Title = x.Title,
            Caption = x.Description,
            Photos = x.Photos?.Select(p => p.Path).ToList(),
            Photo = x.Photo?.Path,
            Region = x.Region?.Name,
            Facilities = x.Facilities?.Select(f => new FacilityItemDto
            {
                Id = f.Id,
                Name = f.Name,
                Icon = f.Icon?.Path
            }).ToList(),
            Languages = x.Languages?.Select(l => l.Name).ToList(),
            RatingAverage = x.RatingAverage,
            AverageCheck = x.AverageCheck,
            Price = x.Price,
            PriceInDollar = x.PriceInDollar,
            viewType = x.Category?.ViewType ?? default(ViewType),
            isFavorite = dbContext.Favorites.Any(f => f.UserId == userId && f.ContentId == x.Id)
        };
    }



    public async virtual Task<ContentDto> GetContent(long ContentId, long userId, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);

        var contentQuery = dbContext.Database.SqlQueryRaw<string>($"""
                SELECT jsonb_build_object(
                    'Id', c."Id",
                    'Title',  c."Title",
                    'Description',  c."Description",
                    'CategoryId',  c."CategoryId",
                    'CategoryName', cat."Name",
                    'ViewType', cat."ViewType",
                    'WorkingHours', jsonb_build_object('Name', COALESCE(cat."FieldNames"->>'WorkingHours', 'WorkingHours') , 'Value', c."WorkingHours"),
                    'Location', jsonb_build_object('Name', COALESCE(cat."FieldNames"->>'Location', 'Location'), 'Value', (ST_AsGeoJSON(c."Location")::jsonb)->'coordinates'),
                    'Facilities', jsonb_build_object('Name', COALESCE(cat."FieldNames"->>'Facilities', 'Facilities')  , 'Value',
                        (SELECT jsonb_agg(jsonb_build_object(
                            'Id', fac."Id",
                            'Name', fac."Name",
                            'Icon', F2."Path"
                        ))
                        FROM "ContentEntityFacilityEntity" cf
                        JOIN "Facilities" fac ON fac."Id" = cf."FacilitiesId" AND cf."FacilitiesLocale" = c."Locale"
                        LEFT JOIN "Files" F2 ON F2."Id" = fac."IconId"
                        WHERE cf."ContentsId" = c."Id" AND c."Locale" = fac."Locale")
                    ),
                    'Languages', jsonb_build_object(
                    'Name', COALESCE(cat."FieldNames"->>'Languages', 'Languages'),
                    'Value', (
                        SELECT jsonb_agg(DISTINCT l."Name")  -- Добавляем DISTINCT
                        FROM "ContentEntityLanguageEntity" cl
                        JOIN "Languages" l ON l."Id" = cl."LanguagesId"
                        WHERE cl."ContentsId" = c."Id" AND l."Locale" = c."Locale"
                    )
                ),
                    'Files', jsonb_build_object('Name', COALESCE(cat."FieldNames"->>'Files', 'Files'), 'Value',
                        (SELECT jsonb_agg(fil."Path")
                        FROM "ContentEntityFileEntity" cfe
                        JOIN "Files" fil ON fil."Id" = cfe."FilesId"
                        WHERE cfe."ContentFilesId" = c."Id" and cfe."ContentFilesLocale" = c."Locale")
                    ),
                    'Attachments', jsonb_build_object(
                    'Name', COALESCE(cat."FieldNames"->>'Files', 'Files'),
                    'Value',
                    (
                        SELECT jsonb_agg(jsonb_build_object(
                            'Name', regexp_replace(fil."Name", '\.[^.]+$', ''),
                            'Icon', '/Images/attachment.png',
                            'Files', fil."Path"
                        ))
                        FROM "ContentEntityFileEntity" cfe
                        JOIN "Files" fil ON fil."Id" = cfe."FilesId"
                        WHERE cfe."ContentFilesId" = c."Id" AND cfe."ContentFilesLocale" = c."Locale"
                    )
                ),
                    'Photos',
                        (SELECT jsonb_agg(fil."Path")
                        FROM "ContentEntityFileEntity1" cfp
                        JOIN "Files" fil ON fil."Id" = cfp."PhotosId"
                        WHERE cfp."ContentPhotosId" = c."Id" and cfp."ContentPhotosLocale" = c."Locale"
                    ),
                    'Photo',  f."Path",
                    'Contacts', jsonb_build_object('Name', COALESCE(cat."FieldNames"->>'Contacts', 'Contacts') , 'Value', c."Contacts"),
                    'Region', r."Name",
                    'RegionId', r."Id",
                    'RatingAverage', c."RatingAverage",
                    'ReviewCount', (
                    SELECT COUNT(*)
                    FROM "Reviews" rev
                    WHERE rev."ContentEntityId" = c."Id" AND rev."ContentEntityLocale" = c."Locale"
                    ),
                    'AverageCheck', c."AverageCheck",
                    'Price',  c."Price",
                    'PriceInDollar',  c."PriceInDollar",
                    'Address',  c."Address",
                    'IsFavorite',
                        EXISTS (
                            SELECT 1
                            FROM "Favorites" fav
                            WHERE fav."UserId" = {userId} AND fav."ContentId" = c."Id"
                        )

                ) AS "Value"
                FROM "Contents" c
                LEFT JOIN "Categories" cat ON cat."Id" = c."CategoryId" AND cat."Locale" = c."Locale"
                LEFT JOIN "Regions" r ON r."Id" = c."RegionId" AND r."Locale" = c."RegionLocale"
                LEFT JOIN "Files" f ON f."Id" = c."PhotoId"
                WHERE c."Id" = {ContentId} AND c."Locale" = '{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}'
                """);

        var str = await contentQuery.FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (str != null)
        {
            var content = JsonSerializer.Deserialize<ContentDto>(str!)!;

            return content;
        }
        throw new NotFoundException("not found");
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
        .Include(x => x.Region)
        .Include(x => x.Facilities!).ThenInclude(x => x.Icon)

        .Where(x => x.Id == Id);
        return content == null ? throw new ValidationException("ContentEntity Not Found") : content.ToList().MapToViewList();
    }

    #endregion
    #region Mutations
    long maxId;
    public async virtual Task Create(CreateContentCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            _ = await categoryService.Invalidate();
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
            dbContext.Database.ExecuteSqlInterpolated($"UPDATE  \"Contents\" set \"Recommended\" = false where \"CategoryId\" = {fContent.CategoryId} and \"RegionId\" = {fContent.RegionId} ;");
        }

        if (command.Entity.First().GlobalRecommended)
        {
            dbContext.Database.ExecuteSqlInterpolated($"UPDATE  \"Contents\" set \"GlobalRecommended\" = false where \"CategoryId\" = {fContent.CategoryId};");
        }


        await dbContext.SaveChangesAsync(cancellationToken);

    }

    public async virtual Task AddRequest(AddRequestCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        var user = await auth.GetUser(command.Session);

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var request = command.ContentRequest.MapToRequest();
            dbContext.Add(request);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    public async virtual Task Delete(DeleteContentCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            _ = await categoryService.Invalidate();
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
        .Include(x => x.Region)
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
            _ = await categoryService.Invalidate();
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
        .Include(x => x.Region)
        .Where(x => x.Id == con.Id).ToList() ?? throw new ValidationException("ContentEntity Not Found");

        foreach (var item in command.Entity)
        {
            var cont = content.First(x => x.Locale == item.Locale);
            Reattach(cont, item, dbContext);
            dbContext.Entry(cont).State = EntityState.Modified;
        }
        var fContent = command.Entity.First();
        if (fContent.Recommended)
        {
            dbContext.Database.ExecuteSqlInterpolated($"UPDATE  \"Contents\" set \"Recommended\" = false where \"CategoryId\" = {fContent.CategoryId} and \"RegionId\" = {fContent.RegionId} ;");
        }

        if (fContent.GlobalRecommended)
        {
            dbContext.Database.ExecuteSqlInterpolated($"UPDATE  \"Contents\" set \"GlobalRecommended\" = false where \"CategoryId\" = {fContent.CategoryId};");
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

        if (contentView.RegionView != null)
        {
            content.Region = dbContext.Regions.First(x => x.Id == contentView.RegionView.Id && x.Locale == contentView.Locale);
        }
        else
        {
            content.Region = contentView.RegionView?.MapFromView();
        }

        content.RatingAverage = contentView.RatingAverage;

        if (content.Category != null)
        {
            var cat = dbContext.Categories
            .First(x => x.Id == content.Category.Id && x.Locale == content.Category.Locale);
            content.Category = cat;
            content.CategoryId = cat.Id;
            content.CategoryLocale = cat.Locale;
        }

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

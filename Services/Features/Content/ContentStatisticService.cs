using ActualLab.Fusion.Authentication;
using ActualLab.Fusion.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using myuzbekistan.Shared;
using System.Globalization;

namespace myuzbekistan.Services;

public class ContentStatisticService(
    IServiceProvider services,
    ICategoryService categoryService,
    ILogger<ContentService> logger,
    IAuth auth)
    : DbServiceBase<AppDbContext>(services), IContentStatisticService
{
    public async virtual Task<StatisticSummaryView> GetSummary(StatisticFilter filter, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        List<string> excluded = ["About Uzbekistan", "Useful tips"];
        var categoryCount = await dbContext.Categories.Where(x=> x.Locale == "en" && x.Status == ContentStatus.Active
        && !excluded.Contains(x.Name)
        ).Select(x => x.Id).CountAsync(cancellationToken);

        
        var contentPerCategory = await dbContext.Contents
            .Where(x => x.Locale == "en" && x.Status == ContentStatus.Active && !excluded.Contains(x.Category.Name))
            .GroupBy(x => new { x.CategoryId, x.Category.Name })
            .Select(g => new CategoryContentCount
            {
                CategoryId = g.Key.CategoryId,
                CategoryName = g.Key.Name,
                ContentCount = g.Count()
            }).ToListAsync(cancellationToken);

        var userCount = await dbContext.Users.CountAsync(cancellationToken);

        var contentRequests = dbContext.ContentRequests.AsQueryable();
        var favorites = dbContext.Favorites.AsQueryable();

        if (filter?.StartDate != null && filter.EndDate != null)
        {
            contentRequests = contentRequests
                .Where(x => x.CreatedAt >= filter.StartDate && x.CreatedAt <= filter.EndDate);

            favorites = favorites
                .Where(x => x.CreatedAt >= filter.StartDate && x.CreatedAt <= filter.EndDate);
        }

        var favoritesPerContent = await favorites
            .GroupBy(f => new { f.ContentId, f.Content.Title })
            .Select(g => new FavoriteStat
            {
                ContentId = g.Key.ContentId,
                ContentName = g.Key.Title,
                Count = g.Count()
            }).ToListAsync(cancellationToken);

        var facilityCount = await dbContext.Facilities.Where(x=>x.Locale == LangHelper.currentLocale) .CountAsync(cancellationToken);

        var topRequests = await contentRequests
            .GroupBy(x => new { x.CategoryId, x.CategoryName, x.ContentId, x.ContentName })
            .Select(g => new TopRequestedPlace
            {
                CategoryId = g.Key.CategoryId,
                CategoryName = g.Key.CategoryName!,
                ContentId = g.Key.ContentId,
                ContentName = g.Key.ContentName!,
                RequestCount = g.Count()
            })
            .OrderByDescending(x => x.RequestCount)
            .ToListAsync(cancellationToken);

        var requestsByDate = await contentRequests
            .GroupBy(x => new { x.CategoryId, x.CategoryName, x.ContentId, x.ContentName, Date = x.CreatedAt.Date })
            .Select(g => new CategoryRequestByDate
            {
                CategoryId = g.Key.CategoryId,
                CategoryName = g.Key.CategoryName!,
                ContentId = g.Key.ContentId,
                ContentName = g.Key.ContentName!,
                Date = g.Key.Date,
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);

        var totalFileSize = await dbContext.Files.SumAsync(f => (long?)f.Size, cancellationToken) ?? 0L;

        return new StatisticSummaryView
        {
            CategoryCount = categoryCount,
            ContentPerCategory = contentPerCategory,
            UserCount = userCount,
            FavoritePerContent = favoritesPerContent,
            FacilityCount = facilityCount,
            TopRequestedPlaces = topRequests,
            RequestsByDate = requestsByDate,
            TotalFileSizeInMb = Math.Round(totalFileSize / 1024.0 / 1024.0, 2)
        };
    }


    public async virtual Task<List<ContentRequestView>> GetAll(long CategoryId, TableOptions options, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);

        var content = dbContext.ContentRequests.AsQueryable();

        if (!string.IsNullOrEmpty(options.Search))
        {
            var search = options.Search.ToLower(CultureInfo.InvariantCulture);
            content = content.Where(x =>
                x.ContentName!.ToLower().Contains(search) ||
                x.CategoryName!.ToLower().Contains(search) ||
                x.RegionName!.ToLower().Contains(search));
        }

        if (CategoryId != 0)
            content = content.Where(x => x.CategoryId == CategoryId);

        var result = await content.ToListAsync(cancellationToken);
        return result.Select(x => x.MapToRequestV()).ToList();
    }
}

// Вспомогательные DTO для возврата статистики


using Microsoft.EntityFrameworkCore;
using ActualLab.Fusion;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using ActualLab.Async;
using System.Reactive;
namespace myuzbekistan.Services;

public class ReviewService(IServiceProvider services, IDbContextFactory<ApplicationDbContext> identityDbFactory) : DbServiceBase<AppDbContext>(services), IReviewService 
{
    private readonly IDbContextFactory<ApplicationDbContext> _identityDbFactory = identityDbFactory;

    #region Queries
    [ComputeMethod]
    public async virtual Task<TableResponse<ReviewView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var review = from s in dbContext.Reviews select s;

        if (!string.IsNullOrEmpty(options.Search))
            review = review.Where(s => s.Comment != null && s.Comment.Contains(options.Search));

        Sorting(ref review, options);
        var count = await review.AsNoTracking().CountAsync(cancellationToken);
        var items = await review.AsNoTracking().Paginate(options).ToListAsync(cancellationToken);
        return new TableResponse<ReviewView>() { Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<TableResponse<ReviewView>> GetByContent(long contentId, TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var review = dbContext.Reviews.Where(r => r.ContentEntityId == contentId);

        if (!string.IsNullOrEmpty(options.Search))
            review = review.Where(r => r.Comment != null && r.Comment.Contains(options.Search));

        Sorting(ref review, options);
        var count = await review.AsNoTracking().CountAsync(cancellationToken);
        var items = await review.AsNoTracking().Paginate(options).ToListAsync(cancellationToken);
        var views = items.MapToViewList();

        // Enrich with identity user data
        var userIds = views.Select(v => v.UserId).Distinct().ToList();
        if (userIds.Count > 0)
        {
            await using var identityDb = await _identityDbFactory.CreateDbContextAsync(cancellationToken);
            var users = await identityDb.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new { u.Id, u.UserName, u.ProfilePictureUrl })
                .ToListAsync(cancellationToken);
            var dict = users.ToDictionary(u => u.Id);
            foreach (var v in views)
            {
                if (dict.TryGetValue(v.UserId, out var u))
                {
                    v.UserName = u.UserName;
                    v.Avatar = u.ProfilePictureUrl;
                }
            }
        }

        return new TableResponse<ReviewView> { Items = views, TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<ReviewView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var review = await dbContext.Reviews.FirstOrDefaultAsync(x => x.Id == Id, cancellationToken);
        return review == null ? throw new NotFoundException("ReviewEntity Not Found") : review.MapToView();
    }
    #endregion

    #region Mutations
    public async virtual Task Create(CreateReviewCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var review = new ReviewEntity();
        Reattach(review, command.Entity, dbContext);
        dbContext.Update(review);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Rating recompute placeholder (commented original logic)
        // var avg = dbContext.Reviews.Where(x => x.ContentEntityId == command.Entity.ContentEntityId).Average(x => x.Rating);
        _ = dbContext.Contents.Where(x => x.Id == command.Entity.ContentEntityId).ToList();
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async virtual Task Delete(DeleteReviewCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var review = await dbContext.Reviews.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);
        if (review == null) throw new ValidationException("ReviewEntity Not Found");
        dbContext.Remove(review);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async virtual Task Update(UpdateReviewCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var review = await dbContext.Reviews.FirstOrDefaultAsync(x => x.Id == command.Entity!.Id, cancellationToken);
        if (review == null) throw new ValidationException("ReviewEntity Not Found");
        Reattach(review, command.Entity, dbContext);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region Helpers
    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    private void Reattach(ReviewEntity review, ReviewView reviewView, AppDbContext dbContext)
    {
        ReviewMapper.From(reviewView, review);
        review.ContentEntityLocale = "en"; // default locale placeholder
    }

    private void Sorting(ref IQueryable<ReviewEntity> review, TableOptions options) => review = options.SortLabel switch
    {
        "Comment" => review.Ordering(options, o => o.Comment!),
        "Rating" => review.Ordering(options, o => o.Rating),
        "Id" => review.Ordering(options, o => o.Id),
        _ => review.OrderBy(o => o.Id),
    };
    #endregion
}

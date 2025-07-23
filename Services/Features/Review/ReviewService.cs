using Microsoft.EntityFrameworkCore;
using ActualLab.Fusion;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using ActualLab.Async;
using System.Reactive;
namespace myuzbekistan.Services;

public class ReviewService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), IReviewService 
{
    #region Queries
    [ComputeMethod]
    public async virtual Task<TableResponse<ReviewView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var review = from s in dbContext.Reviews select s;

        if (!String.IsNullOrEmpty(options.Search))
        {
            review = review.Where(s => 
                     s.Comment.Contains(options.Search)
            );
        }

        Sorting(ref review, options);
        
        var count = await review.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await review.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<ReviewView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<ReviewView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var review = await dbContext.Reviews
        .FirstOrDefaultAsync(x => x.Id == Id);
        
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
        ReviewEntity review=new ReviewEntity();
        Reattach(review, command.Entity, dbContext); 
        
        dbContext.Update(review);
        await dbContext.SaveChangesAsync(cancellationToken);

        //var avg = dbContext.Reviews.Where(x=>x.ContentEntityId == command.Entity.ContentEntityId).Average(x => x.Rating);
        var contents = dbContext.Contents.Where(x => x.Id == command.Entity.ContentEntityId).ToList();
        //foreach (var item in contents)
        //{
        //    item.RatingAverage = Math.Round(avg,2);
        //}

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
        var review = await dbContext.Reviews
        .FirstOrDefaultAsync(x => x.Id == command.Id);
        if (review == null) throw  new ValidationException("ReviewEntity Not Found");
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
        var review = await dbContext.Reviews
        .FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

        if (review == null) throw  new ValidationException("ReviewEntity Not Found"); 

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

        review.ContentEntityLocale = "en";


    }

    private void Sorting(ref IQueryable<ReviewEntity> review, TableOptions options) => review = options.SortLabel switch
    {
        "Comment" => review.Ordering(options, o => o.Comment),
        "Rating" => review.Ordering(options, o => o.Rating),
        "Id" => review.Ordering(options, o => o.Id),
        _ => review.OrderBy(o => o.Id),
        
    };
    #endregion
}

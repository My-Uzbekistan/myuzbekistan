using myuzbekistan.Services;

public class CardColorService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), ICardColorService 
{
    #region Queries

    [ComputeMethod]
    public async virtual Task<TableResponse<CardColorView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var cardcolor = from s in dbContext.CardColors select s;

        Sorting(ref cardcolor, options);
        
        cardcolor = cardcolor.Include(x => x.Image);
        var count = await cardcolor.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await cardcolor.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<CardColorView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<TableResponse<CardColorViewApi>> GetAllApi(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var cardcolor = from s in dbContext.CardColors select s;

        Sorting(ref cardcolor, options);

        cardcolor = cardcolor.Include(x => x.Image);
        var count = await cardcolor.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await cardcolor.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<CardColorViewApi>() { Items = items.MapToApiList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<CardColorView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var cardcolor = await dbContext.CardColors
            .Include(x => x.Image)
            .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken)
            ?? throw  new ValidationException("CardColorEntity Not Found");
        
        return cardcolor.MapToView();
    }

    #endregion

    #region Mutations

    public async virtual Task Create(CreateCardColorCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        CardColorEntity cardcolor = new();
        Reattach(cardcolor, command.Entity, dbContext); 
        
        dbContext.Update(cardcolor);
        await dbContext.SaveChangesAsync(cancellationToken);

    }

    public async virtual Task Update(UpdateCardColorCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var cardcolor = await dbContext.CardColors
            .Include(x=>x.Image)
            .FirstOrDefaultAsync(x => x.Id == command.Entity.Id, cancellationToken)
            ?? throw  new ValidationException("CardColorEntity Not Found");

        Reattach(cardcolor, command.Entity, dbContext);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async virtual Task Delete(DeleteCardColorCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var cardcolor = await dbContext.CardColors
            .Include(x=>x.Image)
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw  new ValidationException("CardColorEntity Not Found");
        dbContext.Remove(cardcolor);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    private static void Reattach(CardColorEntity cardcolor, CardColorView cardcolorView, AppDbContext dbContext)
    {
        CardColorMapper.From(cardcolorView, cardcolor);

        if(cardcolor.Image != null)
        cardcolor.Image = dbContext.Files
        .First(x => x.Id == cardcolor.Image.Id);
    }

    private static void Sorting(ref IQueryable<CardColorEntity> cardcolor, TableOptions options) 
        => cardcolor = options.SortLabel switch
        {
            "Id" => cardcolor.Ordering(options, o => o.Id),
            _ => cardcolor.OrderBy(o => o.Id),
        
        };

    #endregion
}

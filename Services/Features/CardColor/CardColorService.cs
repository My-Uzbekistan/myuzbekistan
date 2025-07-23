namespace myuzbekistan.Services;

public class CardColorService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), ICardColorService 
{
    #region Queries

    [ComputeMethod]
    public async virtual Task<TableResponse<CardColorView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var cardcolor = from s in dbContext.CardColors select s;

        if (!string.IsNullOrEmpty(options.Search))
        {
            cardcolor = cardcolor.Where(s => 
                     s.Name.Contains(options.Search)
                    || s.ColorCode.Contains(options.Search)
            );
        }

        Sorting(ref cardcolor, options);
        
        var count = await cardcolor.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await cardcolor.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<CardColorView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<CardColorView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var cardcolor = await dbContext.CardColors
            .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken)
            ?? throw new NotFoundException("CardColorEntity Not Found");
        
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
            .FirstOrDefaultAsync(x => x.Id == command.Entity.Id, cancellationToken)
            ?? throw new NotFoundException("CardColorEntity Not Found");

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
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw new NotFoundException("CardColorEntity Not Found");
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

    }

    private static void Sorting(ref IQueryable<CardColorEntity> cardcolor, TableOptions options) 
        => cardcolor = options.SortLabel switch
        {
            "Name" => cardcolor.Ordering(options, o => o.Name),
            "ColorCode" => cardcolor.Ordering(options, o => o.ColorCode),
            "Id" => cardcolor.Ordering(options, o => o.Id),
            _ => cardcolor.OrderBy(o => o.Id),
        
        };

    #endregion
}

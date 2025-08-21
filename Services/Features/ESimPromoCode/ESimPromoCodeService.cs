namespace myuzbekistan.Services;

public class ESimPromoCodeService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), IESimPromoCodeService
{
    #region Queries

    public async virtual Task<TableResponse<ESimPromoCodeView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var promoCode = from s in dbContext.ESimPromoCodes select s;

        Sorting(ref promoCode, options);

        var count = await promoCode.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await promoCode.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<ESimPromoCodeView>() { Items = items.MapToViewList(), TotalItems = count };
    }

    public async virtual Task<ESimPromoCodeView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var promoCode = await dbContext.ESimPromoCodes
            .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken)
            ?? throw new ValidationException("ESimPromoCodeEntity Not Found");

        return promoCode.MapToView();
    }

    #endregion

    #region Mutations

    public async virtual Task Create(CreateESimPromoCodeCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        ESimPromoCodeEntity promoCode = new();
        Reattach(promoCode, command.Entity, dbContext);
        promoCode.StartDate = command.Entity.StartDate!.Value.ToUtc();
        promoCode.EndDate = command.Entity.EndDate!.Value.ToUtc();
        dbContext.Update(promoCode);
        await dbContext.SaveChangesAsync(cancellationToken);

    }

    public async virtual Task Update(UpdateESimPromoCodeCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var promoCode = await dbContext.ESimPromoCodes
            .FirstOrDefaultAsync(x => x.Id == command.Entity.Id, cancellationToken)
            ?? throw new ValidationException("ESimPromoCodeEntity Not Found");

        Reattach(promoCode, command.Entity, dbContext);

        promoCode.StartDate = command.Entity.StartDate!.Value.ToUtc();
        promoCode.EndDate = command.Entity.EndDate!.Value.ToUtc();
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async virtual Task Delete(DeleteESimPromoCodeCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var promoCode = await dbContext.ESimPromoCodes
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw new ValidationException("ESimPromoCodeEntity Not Found");
        dbContext.Remove(promoCode);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    private static void Reattach(ESimPromoCodeEntity promoCode, ESimPromoCodeView promoCodeView, AppDbContext dbContext)
    {
        ESimPromoCodeMapper.From(promoCodeView, promoCode);
    }

    private static void Sorting(ref IQueryable<ESimPromoCodeEntity> promoCode, TableOptions options)
        => promoCode = options.SortLabel switch
        {
            "Id" => promoCode.Ordering(options, o => o.Id),
            _ => promoCode.OrderBy(o => o.Id),
        };

    #endregion
}

namespace myuzbekistan.Services;

public class ESimOrderService(IServiceProvider services,
    IUserService userService) 
    : DbServiceBase<AppDbContext>(services), IESimOrderService
{
    #region Queries

    public async virtual Task<TableResponse<ESimOrderView>> GetAll(TableOptions options, Session? session, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);

        var esimOrder = from s in dbContext.ESimOrders select s;
        if (session is not null)
        {
            var user = await userService.GetUserAsync(session, cancellationToken)
                ?? throw new NotFoundException("User not found");
            esimOrder = esimOrder.Where(x => x.UserId == user.Id);
        }

        Sorting(ref esimOrder, options);

        var count = await esimOrder.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await esimOrder.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<ESimOrderView>() { Items = items.MapToViewList(), TotalItems = count };
    }

    public async virtual Task<ESimOrderView> Get(long Id, Session? session, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var esimOrder = await dbContext.ESimOrders
            .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken)
            ?? throw new ValidationException("ESimOrderEntity Not Found");

        if (session is not null)
        {
            var user = await userService.GetUserAsync(session, cancellationToken)
                ?? throw new NotFoundException("User not found");
            if (esimOrder.UserId != user.Id)
                throw new BadRequestException("You do not have permission to view this order.");
        }

        return esimOrder.MapToView();
    }

    #endregion

    #region Mutations

    public async virtual Task Create(CreateESimOrderCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        ESimOrderEntity esimOrder = new();
        Reattach(esimOrder, command.Entity, dbContext);

        dbContext.Update(esimOrder);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    private static void Reattach(ESimOrderEntity esimOrder, ESimOrderView esimOrderView, AppDbContext dbContext)
    {
        ESimOrderMapper.From(esimOrderView, esimOrder);
    }

    private static void Sorting(ref IQueryable<ESimOrderEntity> esimOrder, TableOptions options)
        => esimOrder = options.SortLabel switch
        {
            "Id" => esimOrder.Ordering(options, o => o.Id),
            _ => esimOrder.OrderBy(o => o.Id)
        };

    #endregion
}

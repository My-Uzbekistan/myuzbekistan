using myuzbekistan.Services;

public class MerchantService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), IMerchantService
{
    #region Queries

    [ComputeMethod]
    public async virtual Task<TableResponse<MerchantView>> GetAll(long? merchantCategoryId, TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var merchant = from s in dbContext.Merchants select s;

        if (!string.IsNullOrEmpty(options.Search))
        {
            merchant = merchant.Where(s =>
                     s.Name != null && s.Name.Contains(options.Search)
                    || s.Description != null && s.Description.Contains(options.Search)
                    || s.Address != null && s.Address.Contains(options.Search)
                    || s.MXIK != null && s.MXIK.Contains(options.Search)
                    || s.WorkTime != null && s.WorkTime.Contains(options.Search)
                    || s.Phone != null && s.Phone.Contains(options.Search)
                    || s.Responsible.Contains(options.Search)
            );
        }
        if (merchantCategoryId.HasValue && merchantCategoryId.Value > 0)
            merchant = merchant.Where(x => x.MerchantCategory.Id == merchantCategoryId);

        Sorting(ref merchant, options);

        merchant = merchant.Include(x => x.Logo);
        merchant = merchant.Include(x => x.MerchantCategory);
        var count = await merchant.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await merchant.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<MerchantView>() { Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<MerchantView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var merchant = await dbContext.Merchants
            .Include(x => x.Logo)
            .Include(x => x.MerchantCategory)
            .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken)
            ?? throw new ValidationException("MerchantEntity Not Found");

        return merchant.MapToView();
    }

    #endregion

    #region Mutations

    public async virtual Task Create(CreateMerchantCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        MerchantEntity merchant = new();
        Reattach(merchant, command.Entity, dbContext);

        dbContext.Update(merchant);
        await dbContext.SaveChangesAsync(cancellationToken);

    }

    public async virtual Task Update(UpdateMerchantCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var merchant = await dbContext.Merchants
            .Include(x => x.Logo)
            .Include(x => x.MerchantCategory)
            .FirstOrDefaultAsync(x => x.Id == command.Entity.Id, cancellationToken)
            ?? throw new ValidationException("MerchantEntity Not Found");

        Reattach(merchant, command.Entity, dbContext);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async virtual Task Delete(DeleteMerchantCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var merchant = await dbContext.Merchants
            .Include(x => x.Logo)
            .Include(x => x.MerchantCategory)
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw new ValidationException("MerchantEntity Not Found");
        dbContext.Remove(merchant);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    private static void Reattach(MerchantEntity merchant, MerchantView merchantView, AppDbContext dbContext)
    {
        MerchantMapper.From(merchantView, merchant);

        if (merchant.Logo != null)
            merchant.Logo = dbContext.Files
            .First(x => x.Id == merchant.Logo.Id);
        if (merchant.MerchantCategory != null)
            merchant.MerchantCategory = dbContext.MerchantCategories
            .First(x => x.Id == merchant.MerchantCategory.Id);
    }

    private static void Sorting(ref IQueryable<MerchantEntity> merchant, TableOptions options)
        => merchant = options.SortLabel switch
        {
            "Logo" => merchant.Ordering(options, o => o.Logo),
            "Name" => merchant.Ordering(options, o => o.Name),
            "Description" => merchant.Ordering(options, o => o.Description),
            "Address" => merchant.Ordering(options, o => o.Address),
            "Mfi" => merchant.Ordering(options, o => o.MXIK),
            "WorkTime" => merchant.Ordering(options, o => o.WorkTime),
            "Phone" => merchant.Ordering(options, o => o.Phone),
            "Responsible" => merchant.Ordering(options, o => o.Responsible),
            "Status" => merchant.Ordering(options, o => o.Status),
            "MerchantCategory" => merchant.Ordering(options, o => o.MerchantCategory),
            "Id" => merchant.Ordering(options, o => o.Id),
            _ => merchant.OrderBy(o => o.Id),

        };

    #endregion
}

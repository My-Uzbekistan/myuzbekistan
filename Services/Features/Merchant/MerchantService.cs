using ActualLab.Api;
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

        #region Search by Language

        if (!String.IsNullOrEmpty(options.Lang))
            merchant = merchant.Where(x => x.Locale.Equals(options.Lang));

        #endregion

        Sorting(ref merchant, options);

        merchant = merchant.Include(x => x.Logo);
        merchant = merchant.
             Include(x => x.MerchantCategory)
            .ThenInclude(x => x.ServiceType)
            .Include(x => x.MerchantCategory)
            .ThenInclude(x => x.Logo);
        var count = await merchant.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await merchant.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<MerchantView>() { Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<TableResponse<MerchantResponse>> GetAllByApi(TableOptions options, CancellationToken cancellationToken = default)
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

        #region Search by Language

            merchant = merchant.Where(x => x.Locale.Equals(LangHelper.currentLocale));

        #endregion

        Sorting(ref merchant, options);

        merchant = merchant.Include(x => x.Logo);
        merchant = merchant.
             Include(x => x.MerchantCategory)
            .ThenInclude(x => x.ServiceType)
            .Include(x => x.MerchantCategory)
            .ThenInclude(x => x.Logo);
        var count = await merchant.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await merchant.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<MerchantResponse>() { Items = items.MapToResponseList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<MerchantResponse> GetByApi(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var merchant = dbContext.Merchants
            .Include(x => x.Logo)
            .Include(x => x.MerchantCategory)
            .ThenInclude(x => x.ServiceType)
            .Include(x => x.MerchantCategory)
            .ThenInclude(x => x.Logo)
            .Where(x => x.Id == Id);
            
            

        #region Search by Language
            merchant = merchant.Where(x => x.Locale.Equals(LangHelper.currentLocale));
        #endregion
        var merchantC = await merchant.FirstOrDefaultAsync(cancellationToken: cancellationToken)
        ?? throw new ValidationException("MerchantEntity Not Found");

        return merchantC.MapToResponse();
    }

    [ComputeMethod]
    public async virtual Task<List<MerchantView>> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var merchant = await dbContext.Merchants
            .Include(x => x.Logo)
            .Include(x => x.MerchantCategory)
            .ThenInclude(x => x.ServiceType)
            .Include(x => x.MerchantCategory)
            .ThenInclude(x => x.Logo)
            .Where(x => x.Id == Id)
            .ToListAsync(cancellationToken)
            ?? throw new ValidationException("MerchantEntity Not Found");

        return merchant.MapToViewList();
    }

    #endregion

    #region Mutations
    long maxId;
    public async virtual Task Create(CreateMerchantCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        maxId = dbContext.Merchants.Count() == 0 ? 0 : dbContext.Merchants.Max(x => x.Id);
        maxId++;
        foreach (var item in command.Entity)
        {
            MerchantEntity category = new MerchantEntity();
            Reattach(category, item, dbContext);
            category.Id = maxId;
            dbContext.Add(category);

        }
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
        var merch = command.Entity.First();
        var merchant = dbContext.Merchants
        .Include(x => x.Logo)
        .Where(x => x.Id == merch.Id).ToList();

        if (merchant == null) throw new ValidationException("MerchantEntity Not Found");

        foreach (var item in command.Entity)
        {
            Reattach(merchant.First(x => x.Locale == item.Locale), item, dbContext);
            dbContext.Update(merchant.First(x => x.Locale == item.Locale));
        }
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async virtual Task UpdateToken(UpdateMerchantTokenCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var merchants = dbContext.Merchants
        .Include(x => x.Logo)
        .Where(x => x.Id == command.MerchantId).ToList();


        foreach (var item in merchants)
        {
            item.Token = command.Token;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async virtual Task AddChatId(MerchantAddChatIdCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var merchants = dbContext.Merchants
        .Include(x => x.Logo)
        .Where(x => x.Token == command.Token).ToList();


        foreach (var item in merchants)
        {
            item.ChatIds.Add(command.ChatId);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task ClearChatId(MerchantClearChatIdCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var merchants = dbContext.Merchants
        .Include(x => x.Logo)
        .Where(x => x.Id == command.MerchantId).ToList();

        foreach (var item in merchants)
        {
            item.ChatIds.Clear();
        }

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


        var merchants = dbContext.Merchants
       .Include(x => x.Logo)
       .Where(x => x.Id == command.Id)
       .ToList();
        if (merchants == null) throw new ValidationException("MerchantCategoryEntity Not Found");
        dbContext.RemoveRange(merchants);
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

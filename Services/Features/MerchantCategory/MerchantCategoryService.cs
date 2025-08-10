using DocumentFormat.OpenXml.Office.Word;
using myuzbekistan.Services;

public class MerchantCategoryService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), IMerchantCategoryService
{
    #region Queries

    [ComputeMethod]
    public async virtual Task<TableResponse<MerchantCategoryView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var merchantcategory = from s in dbContext.MerchantCategories select s;

        if (!string.IsNullOrEmpty(options.Search))
        {
            merchantcategory = merchantcategory.Where(s =>
                     s.BrandName != null && s.BrandName.Contains(options.Search)
                    || s.OrganizationName != null && s.OrganizationName.Contains(options.Search)
                    || s.Description != null && s.Description.Contains(options.Search)
                    || s.Inn.Contains(options.Search)
                    || s.AccountNumber.Contains(options.Search)
                    || s.MfO != null && s.MfO.Contains(options.Search)
                    || s.Contract != null && s.Contract.Contains(options.Search)
                    || s.Phone != null && s.Phone.Contains(options.Search)
                    || s.Email != null && s.Email.Contains(options.Search)
                    || s.Address != null && s.Address.Contains(options.Search)
            );
        }

        #region Search by Language

        if (!String.IsNullOrEmpty(options.Lang))
            merchantcategory = merchantcategory.Where(x => x.Locale.Equals(options.Lang));

        #endregion

        Sorting(ref merchantcategory, options);

        merchantcategory = merchantcategory.Include(x => x.Logo);
        merchantcategory = merchantcategory.Include(x => x.Merchants);
        merchantcategory = merchantcategory.Include(x => x.ServiceType);
        var count = await merchantcategory.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await merchantcategory.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<MerchantCategoryView>() { Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<List<MerchantCategoryView>> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var merchantcategory = await dbContext.MerchantCategories
            .Include(x => x.Logo)
            .Include(x => x.Merchants)
            .Include(x => x.ServiceType)
            .Where(x => x.Id == Id).ToListAsync(cancellationToken: cancellationToken);

        return merchantcategory == null ? throw new NotFoundException("MerchantCategoryEntity Not Found") : merchantcategory.MapToViewList();
    }

    #endregion

    #region Mutations

    long maxId;

    public async virtual Task Create(CreateMerchantCategoryCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        maxId = dbContext.MerchantCategories.Count() == 0 ? 0 : dbContext.MerchantCategories.Max(x => x.Id);
        maxId++;
        foreach (var item in command.Entity)
        {
            MerchantCategoryEntity category = new MerchantCategoryEntity();
            Reattach(category, item, dbContext);
            category.Id = maxId;
            dbContext.Add(category);
        }
        await dbContext.SaveChangesAsync(cancellationToken);

    }

    public async virtual Task Update(UpdateMerchantCategoryCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);

        var cat = command.Entity.First();
        var category = dbContext.MerchantCategories
        .Include(x => x.Merchants)
        .Include(x => x.Logo)
        .Include(x => x.ServiceType)
        .Where(x => x.Id == cat.Id).ToList();

        if (category == null) throw new ValidationException("MerchantCategoryEntity Not Found");

        foreach (var item in command.Entity)
        {
            Reattach(category.First(x => x.Locale == item.Locale), item, dbContext);
            dbContext.Update(category.First(x => x.Locale == item.Locale));
        }
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task UpdateToken(UpdateMerchantCategoryTokenCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var merchants = dbContext.MerchantCategories
        .Include(x => x.Logo)
        .Where(x => x.Id == command.MerchantId).ToList();


        foreach (var item in merchants)
        {
            item.Token = command.Token;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async virtual Task AddChatId(MerchantCategoryAddChatIdCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var merchants = await dbContext.MerchantCategories
        .Include(x => x.Logo)
        .Where(x => x.Token == command.Token).ToListAsync();


        foreach (var mc in merchants)
        {
            // гарантируем, что коллекция не null
            var ids = mc.ChatIds ?? new List<string?>();

            if (!ids.Contains(command.ChatId))
                mc.ChatIds = ids.Append(command.ChatId).ToList(); // ← новая ссылка
        }

        dbContext.UpdateRange(merchants);

        dbContext.ChangeTracker.DetectChanges();
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task ClearChatId(MerchantCategoryClearChatIdCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var merchants = dbContext.MerchantCategories
        .Where(x => x.Id == command.MerchantCategoryId).ToList();

        foreach (var item in merchants)
        {
            item.ChatIds.Clear();
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async virtual Task Delete(DeleteMerchantCategoryCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);

        var category = dbContext.MerchantCategories
       .Include(x => x.Merchants)
       .Include(x => x.Logo)
       .Include(x => x.ServiceType)
       .Where(x => x.Id == command.Id)
       .ToList();
        if (category == null) throw new ValidationException("MerchantCategoryEntity Not Found");
        dbContext.RemoveRange(category);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    private static void Reattach(MerchantCategoryEntity merchantcategory, MerchantCategoryView merchantcategoryView, AppDbContext dbContext)
    {
        MerchantCategoryMapper.From(merchantcategoryView, merchantcategory);

        if (merchantcategory.Logo != null)
            merchantcategory.Logo = dbContext.Files
            .First(x => x.Id == merchantcategory.Logo.Id);

        if (merchantcategory.ServiceType != null)
            merchantcategory.ServiceType = dbContext.ServiceTypes
            .First(x => x.Id == merchantcategoryView.ServiceType.Id && x.Locale == merchantcategory.Locale);

        if (merchantcategory.Merchants != null)
            merchantcategory.Merchants = dbContext.Merchants
            .Where(x => merchantcategory.Merchants.Select(tt => tt.Id).ToList().Contains(x.Id) && x.Locale == merchantcategory.Locale).ToList();
    }

    private static void Sorting(ref IQueryable<MerchantCategoryEntity> merchantcategory, TableOptions options)
        => merchantcategory = options.SortLabel switch
        {
            "Logo" => merchantcategory.Ordering(options, o => o.Logo),
            "BrandName" => merchantcategory.Ordering(options, o => o.BrandName),
            "OrganizationName" => merchantcategory.Ordering(options, o => o.OrganizationName),
            "Description" => merchantcategory.Ordering(options, o => o.Description),
            "Inn" => merchantcategory.Ordering(options, o => o.Inn),
            "AccountNumber" => merchantcategory.Ordering(options, o => o.AccountNumber),
            "MfO" => merchantcategory.Ordering(options, o => o.MfO),
            "Contract" => merchantcategory.Ordering(options, o => o.Contract),
            "Discount" => merchantcategory.Ordering(options, o => o.Discount),
            "PayDay" => merchantcategory.Ordering(options, o => o.PayDay),
            "ServiceType" => merchantcategory.Ordering(options, o => o.ServiceType),
            "Phone" => merchantcategory.Ordering(options, o => o.Phone),
            "Email" => merchantcategory.Ordering(options, o => o.Email),
            "Address" => merchantcategory.Ordering(options, o => o.Address),
            "IsVat" => merchantcategory.Ordering(options, o => o.IsVat),
            "Status" => merchantcategory.Ordering(options, o => o.Status),
            "Merchants" => merchantcategory.Ordering(options, o => o.Merchants),
            "Id" => merchantcategory.Ordering(options, o => o.Id),
            _ => merchantcategory.OrderBy(o => o.Id),

        };

    #endregion
}

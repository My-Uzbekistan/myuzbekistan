namespace myuzbekistan.Services;

public class ESimPromoCodeService(IServiceProvider services, IUserService userService) : DbServiceBase<AppDbContext>(services), IESimPromoCodeService
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

    public async virtual Task<(bool IsApplyable, string ErrorMessage)> Verify(string? code, Session? session, long packageId, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);

        var user = await userService.GetUserAsync(session!, cancellationToken)
            ?? throw new NotFoundException("User Not Found");
        var userId = user.Id;

        var promoCode = await dbContext.ESimPromoCodes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => !string.IsNullOrEmpty(code) && x.Code == code.ToUpper(), cancellationToken);
        if (promoCode == null)
        {
            return (false, "PromoCode Not Found");
        }

        var package = await dbContext.ESimPackages
            .AsNoTracking()
            .Include(x => x.PackageDiscountEntity)
            .FirstOrDefaultAsync(x => x.Id == packageId, cancellationToken);

        if (package == null)
        {
            return (false, "Package Not Found");
        }

        if (!promoCode.IsActive)
        {
            return (false, "PromoCode is not active");
        }

        if (promoCode.StartDate.HasValue && promoCode.StartDate.Value > DateTime.UtcNow)
        {
            return (false, "PromoCode is not started yet");
        }

        if (promoCode.EndDate.HasValue && promoCode.EndDate.Value < DateTime.UtcNow)
        {
            return (false, "PromoCode is expired");
        }

        if (promoCode.UsageLimit > 0 && promoCode.AppliedCount >= promoCode.UsageLimit)
        {
            return (false, "PromoCode usage limit exceeded");
        }

        var userAppliedCount = await dbContext.ESimPromoCodeUsages
            .AsNoTracking()
            .CountAsync(x => x.PromoCodeId == promoCode.Id && x.ApplicationUserId == userId, cancellationToken);

        if (promoCode.MaxUsagePerUser > 0 && userAppliedCount >= promoCode.MaxUsagePerUser)
        {
            return (false, "You have already used this PromoCode the maximum number of times allowed");
        }

        if (package.PackageDiscountId.HasValue && package.PackageDiscountEntity != null && package.PackageDiscountEntity.Status != ContentStatus.Active && !promoCode.IsCompatibleWithDiscount)
        {
            return (false, "PromoCode is not compatible with existing package discount");
        }

        return (true, string.Empty);
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
        promoCode.Code = command.Entity.Code!.ToUpper();
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
        promoCode.Code = command.Entity.Code!.ToUpper();
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

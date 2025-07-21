namespace myuzbekistan.Services;

public class ESimOrderService(
    IServiceProvider services,
    IUserService userService,
    IAiraloPackageService airaloPackageService) 
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
            ?? throw new NotFoundException("ESimOrderEntity Not Found");

        if (session is not null)
        {
            var user = await userService.GetUserAsync(session, cancellationToken)
                ?? throw new NotFoundException("User not found");
            if (esimOrder.UserId != user.Id)
                throw new BadRequestException("You do not have permission to view this order.");
        }

        return esimOrder.MapToView();
    }

    public virtual async Task<TableResponse<MyEsimsView>> GetAllEsim(TableOptions options, Session? session, CancellationToken cancellationToken = default)
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

        List<MyEsimsView> result = [];
        foreach (var item in items)
        {
            var package = await dbContext.ESimPackages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PackageId == item.PackageId, cancellationToken)
                ?? throw new NotFoundException("ESimPackage not found");
            var dataUsage = await airaloPackageService.GetOrderPackageStatusAsync(item.Iccid, cancellationToken)
                ?? throw new NotFoundException("OrderPackageStatus not found");

            result.Add(new()
            {
                Id = item.Id,
                CountryName = package.CountryName,
                OperatorName = package.OperatorName,
                DataValume = package.DataVolume,
                RemainingData = dataUsage.Data.Remaining,
                ValidDays = package.ValidDays,
                ImageUrl = package.ImageUrl
            });
        }

        return new() { Items = result, TotalItems = count };
    }

    public virtual async Task<EsimView> GetEsim(long Id, Session? session, CancellationToken cancellationToken = default)
    {
        if (session is null)
        {
            throw new BadRequestException("Session is required to get ESim details.");
        }
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var user = await userService.GetUserAsync(session, cancellationToken)
            ?? throw new NotFoundException("User not found");
        var esimOrder = await dbContext.ESimOrders
            .FirstOrDefaultAsync(x => x.Id == Id && x.UserId == user.Id, cancellationToken)
            ?? throw new NotFoundException("ESimOrderEntity Not Found");
        var package = await dbContext.ESimPackages
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PackageId == esimOrder.PackageId, cancellationToken)
            ?? throw new NotFoundException("ESimPackage not found");
        var dataUsage = await airaloPackageService.GetOrderPackageStatusAsync(esimOrder.Iccid, cancellationToken)
            ?? throw new NotFoundException("OrderPackageStatus not found");
        var otherPackages = await dbContext.ESimPackages
            .AsNoTracking()
            .Where(x => x.CountryCode == package.CountryCode && x.CountryName == package.CountryName)
            .ToListAsync(cancellationToken);

        return new()
        {
            Id = esimOrder.Id,
            ICCID = esimOrder.Iccid,
            PackageId = esimOrder.PackageId,
            CountryName = package.CountryName,
            OperatorName = package.OperatorName,
            DataValume = package.DataVolume,
            ValidDays = package.ValidDays,
            ImageUrl = package.ImageUrl,
            RemainingData = dataUsage.Data.Remaining,
            Status = dataUsage.Data.Status,
            QrCode = esimOrder.QrCode,
            QrCodeUrl = esimOrder.QrCodeUrl,
            QrCodeInstallation = esimOrder.QrCodeInstallation,
            DirectAppleUrl = esimOrder.DirectAppleUrl,
            ManualInstallation = esimOrder.ManualInstallation,
            ActivationDate = esimOrder.ActivationDate?.ToString("yyyy-MM-dd"),
            OtherPackages = otherPackages.MapToViewList(),
        };
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

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

    public virtual async Task<TableResponse<ESimOrderListView>> GetAllList(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);

        var esimOrder = from s in dbContext.ESimOrders select s;

        Sorting(ref esimOrder, options);

        var count = await esimOrder.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await esimOrder.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        List<ESimOrderListView> result = [];
        foreach (var item in items)
        {
            ESimOrderListView view = new()
            {
                Id = item.Id,
                CreatedAt = item.CreatedAt,
            };

            if (item.PromoCodeId > 0)
            {
                var promoCode = await dbContext.ESimPromoCodes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == item.PromoCodeId, cancellationToken)
                    ?? throw new NotFoundException("ESimPromoCode not found");
                view.ESimPromoCodeView = promoCode.MapToView();
            }

            var package = await dbContext.ESimPackages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PackageId == item.PackageId, cancellationToken)
                ?? throw new NotFoundException("ESimPackage not found");
            view.ESimPackageView = package.MapToView();

            var user = await userService.GetAsync(item.UserId, cancellationToken)
                ?? throw new NotFoundException("User not found");
            view.User = new UserView()
            {
                Id = user.Id,
                UserName = user.FullName ?? string.Empty
            };
            result.Add(view);
        }

        return new TableResponse<ESimOrderListView>() { Items = result, TotalItems = count };
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

        if (options.EsimIsActive == true)
        {
            esimOrder = esimOrder.Where(x => x.ActivationDate != null && x.ActivationDate <= DateTime.UtcNow.AddDays(x.Validity));
        }
        else if (options.EsimIsActive == false)
        {
            esimOrder = esimOrder.Where(x => x.ActivationDate == null || x.ActivationDate > DateTime.UtcNow.AddDays(x.Validity));
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
                Voice = package.Voice,
                ActivationDate = item.SimCreatedAt,
                Text = package.Text,
                HasVoicePack = package.HasVoicePack,
                RemainingData = dataUsage.Data.Remaining,
                ValidDays = package.ValidDays,
                ImageUrl = package.ImageUrl ?? string.Empty
            });
        }

        return new() { Items = result, TotalItems = count };
    }

    public virtual async Task<EsimView> GetEsim(long Id, Session? session, CancellationToken cancellationToken = default, bool exploreMore = true, long userId = 0)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        ApplicationUser user;
        ESimOrderEntity? esimOrder = await dbContext.ESimOrders
            .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken)
            ?? throw new NotFoundException("ESimOrderEntity Not Found");
        if (userId != 0 && esimOrder.UserId != userId)
        {
            throw new BadRequestException("You do not have permission to view this order.");
        }
        
        if (session is not null)
        {
            user = await userService.GetUserAsync(session, cancellationToken)
                ?? throw new NotFoundException("User not found");
            if (esimOrder.UserId != user.Id)
            {
                throw new BadRequestException("You do not have permission to view this order.");
            }
        }
        var package = await dbContext.ESimPackages
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PackageId == esimOrder.PackageId, cancellationToken)
            ?? throw new NotFoundException("ESimPackage not found");
        var dataUsage = await airaloPackageService.GetOrderPackageStatusAsync(esimOrder.Iccid, cancellationToken)
            ?? throw new NotFoundException("OrderPackageStatus not found");
        List<ESimPackageEntity> otherPackages = [];
        if (exploreMore)
        {
            otherPackages = await dbContext.ESimPackages
            .AsNoTracking()
            .Where(x => x.CountryCode == package.CountryCode && x.CountryName == package.CountryName)
            .ToListAsync(cancellationToken);
        }

        return new()
        {
            Id = esimOrder.Id,
            ICCID = esimOrder.Iccid,
            PackageId = esimOrder.PackageId,
            CountryName = package.CountryName,
            OperatorName = package.OperatorName,
            DataValume = package.DataVolume,
            Voice = package.Voice,
            Text = package.Text,
            HasVoicePack = package.HasVoicePack,
            ValidDays = package.ValidDays,
            ImageUrl = package.ImageUrl ?? string.Empty,
            RemainingData = dataUsage.Data.Remaining,
            RemainingVoice = dataUsage.Data.RemainingVoice,
            RemainingText = dataUsage.Data.RemainingText,
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

    public async virtual Task<ESimOrderView> Create(CreateESimOrderCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return new();
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        ESimOrderEntity esimOrder = new();
        Reattach(esimOrder, command.Entity, dbContext);
        esimOrder.SimCreatedAt = esimOrder.SimCreatedAt.ToUtc();
        dbContext.Update(esimOrder);
        await dbContext.SaveChangesAsync(cancellationToken);

        return esimOrder.MapToView();
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

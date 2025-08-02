using System.Diagnostics;

namespace myuzbekistan.Services;

public class ESimPackageService(
    IServiceProvider services,
    IAiraloPackageService airaloPackageService,
    IAiraloTokenService airaloTokenService,
    IConfiguration configuration,
    ICurrencyService currencyService,
    IUserService userService,
    IESimOrderService eSimOrderService,
    ICommander commander) 
    : DbServiceBase<AppDbContext>(services), IESimPackageService
{
    #region Queries
    
    public virtual async Task<List<ESimCoverageView>> GetPackageCoverages(long Id, Language language, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var eSimPackage = await dbContext.ESimPackages
            .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken)
            ?? throw new NotFoundException("ESimPackageEntity Not Found");
        if (eSimPackage.Coverage is null || eSimPackage.Coverage.Count == 0)
        {
            return [];
        }

        var esimSlugs = dbContext.ESimSlugs.ToList();
        List<ESimCoverageView> result = [];
        Parallel.ForEach(eSimPackage.Coverage, coverage =>
        {
            var coverageSlug = esimSlugs.FirstOrDefault(x => x.CountryCode == coverage.Code);
            if (coverageSlug != null)
            {
                result.Add(new()
                {
                    Code = coverageSlug.CountryCode ?? string.Empty,
                    Id = coverageSlug.Id,
                    ImageUrl = coverageSlug.ImageUrl ?? string.Empty,
                    Name = language switch
                    {
                        Language.en => coverageSlug.TitleEn,
                        Language.ru => coverageSlug.TitleRu,
                        _ => coverageSlug.TitleUz
                    },
                    Networks = [..coverage.Networks.Select(x => new ESimCoverageNetworkView()
                    {
                        Name = x.Name,
                        Types = x.Types
                    })]
                });
            }
        });

        return result;
    }

    public async virtual Task<UserCountsView> GetCounts(CancellationToken cancellationToken)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var users = await userService.GetAllUsers(new TableOptions() { PageSize = 1 }, cancellationToken);
        var packagesCount = await dbContext.ESimPackages
            .Where(x => x.Status == ContentStatus.Active)
            .CountAsync(cancellationToken: cancellationToken);

        var countriesCount = await dbContext.ESimSlugs.Where(x => x.SlugType == ESimSlugType.Local).CountAsync(cancellationToken);

        return new()
        {
            PackagesCount = packagesCount,
            UsersCount = users.TotalItems,
            CountriesCount = countriesCount
        };
    }

    public async virtual Task<TableResponse<ESimPackageView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);

        var eSimPackage = from s in dbContext.ESimPackages select s;
        eSimPackage = eSimPackage.Include(x => x.ESimSlug)
            .Where(s => (string.IsNullOrEmpty(options.CountrySlug) || s.ESimSlug!.Slug == options.CountrySlug));

        if (!string.IsNullOrEmpty(options.Search))
        {
            eSimPackage = eSimPackage.Where(x =>
                    x.PackageId.Contains(options.Search.ToLower()) ||
                    x.ESimSlug!.TitleUz.Contains(options.Search) ||
                    x.ESimSlug!.TitleEn.Contains(options.Search) ||
                    x.ESimSlug!.TitleRu.Contains(options.Search));
        }

        if (options.HasVoicePack == true)
        {
            eSimPackage = eSimPackage.Where(x => x.HasVoicePack);
        }

        Sorting(ref eSimPackage, options);

        var count = await eSimPackage.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await eSimPackage.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        var views = items.MapToViewList();
        var currency = await currencyService.GetUsdCourse(cancellationToken);
        double rate = double.Parse(currency.Rate.Replace(",", "."), CultureInfo.InvariantCulture);
        foreach (var view in views)
        {
            view.Price = view.Price * rate;
            if (view.PackageDiscountId.HasValue)
            {
                var packageDiscount = await dbContext.PackageDiscounts
                    .FirstOrDefaultAsync(x => x.Id == view.PackageDiscountId, cancellationToken);
                view.PackageDiscountView = packageDiscount?.MapToView();
            }
        }

        return new TableResponse<ESimPackageView>() { Items = views, TotalItems = count };
    }

    public async virtual Task<ESimPackageView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var eSimPackage = await dbContext.ESimPackages
            .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken)
            ?? throw new NotFoundException("ESimPackageEntity Not Found");

        var view = eSimPackage.MapToView();
        var currency = await currencyService.GetUsdCourse(cancellationToken);
        double rate = double.Parse(currency.Rate);
        view.Price = view.Price * rate;
        return view;
    }

    public async virtual Task<TableResponse<ESimPackageClientView>> GetClientViewAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var eSimPackage = from s in dbContext.ESimPackages select s;
        eSimPackage = eSimPackage.Include(x => x.ESimSlug)
            .Where(s => s.ESimSlug!.Slug == options.CountrySlug);

        if (!string.IsNullOrEmpty(options.Search))
        {
            eSimPackage = eSimPackage.Where(x =>
                    x.PackageId.Contains(options.Search.ToLower()) ||
                    x.ESimSlug!.TitleUz.Contains(options.Search) ||
                    x.ESimSlug!.TitleEn.Contains(options.Search) ||
                    x.ESimSlug!.TitleRu.Contains(options.Search));
        }

        Sorting(ref eSimPackage, options);

        var count = await eSimPackage.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await eSimPackage.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        Language language = options.Lang.ConvertToLanguage();
        List<ESimPackageClientView> views = [];
        foreach (var entity in eSimPackage)
        {
            var view = (ESimPackageClientView)entity.MapToView();

            if (entity.PackageDiscountId > 0)
            {
                var packageDiscount = await dbContext.PackageDiscounts
                    .FirstOrDefaultAsync(x => x.Id == entity.PackageDiscountId, cancellationToken);
                view.PackageDiscountView = packageDiscount?.MapToView();
            }

            view.CountryName = language switch
            {
                Language.en => entity.ESimSlug!.TitleEn,
                Language.ru => entity.ESimSlug!.TitleRu,
                _ => entity.ESimSlug!.TitleUz
            };
            view.Countries = [];
            views.Add(view);
        }

        return new TableResponse<ESimPackageClientView>() { Items = views, TotalItems = count };
    }

    public async virtual Task<ESimPackageClientView> GetClientView(long Id, Language language, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var eSimPackage = await dbContext.ESimPackages
            .AsNoTracking()
            .Include(x => x.ESimSlug)
            .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken)
            ?? throw new NotFoundException("ESimPackageEntity Not Found");

        var view = (ESimPackageClientView)eSimPackage.MapToView();
        if (eSimPackage.PackageDiscountId > 0)
        {
            var packageDiscount = await dbContext.PackageDiscounts
                .FirstOrDefaultAsync(x => x.Id == eSimPackage.PackageDiscountId, cancellationToken);
            view.PackageDiscountView = packageDiscount?.MapToView();
        }
        view.CountryName = language switch
        {
            Language.en => eSimPackage.ESimSlug!.TitleEn,
            Language.ru => eSimPackage.ESimSlug!.TitleRu,
            _ => eSimPackage.ESimSlug!.TitleUz
        };

        if (eSimPackage.Locals.Count > 0)
        {
            var slugs = await dbContext.ESimSlugs.Where(x => x.SlugType == ESimSlugType.Local).ToListAsync(cancellationToken);
            view.Countries = [.. eSimPackage.Locals
                .Select(localId => slugs.FirstOrDefault(x => x.Id == localId))
                .Where(slug => slug != null)
                .Select(s => s!.ToView(language))];
        }

        return view;
    }

    public async virtual Task<UserView> GetUserAsync(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        ApplicationUser user = await userService.GetAsync(Id, cancellationToken);

        var userOrders = await dbContext.ESimOrders
            .Where(x => x.UserId == Id)
            .ToListAsync(cancellationToken);
        List<EsimView> userEsims = [];
        foreach (var esim in userOrders)
        {
            var package = await eSimOrderService.GetEsim(esim.Id, null, cancellationToken)
                ?? throw new NotFoundException("ESimOrder Not Found");
            userEsims.Add(package);
        }

        UserView userView = user;
        userView.Orders = userEsims;
        return userView;
    }

    #endregion

    #region Mutations

    public async virtual Task Create(CreateESimPackageCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        ESimPackageEntity eSimPackage = new ESimPackageEntity();
        Reattach(eSimPackage, command.Entity, dbContext);
        eSimPackage.ESimSlugId = command.Entity.SlugId;
        dbContext.Add(eSimPackage);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async virtual Task Update(UpdateESimPackageCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var eSimPackage = await dbContext.ESimPackages
            .FirstOrDefaultAsync(x => x.Id == command.Entity.Id, cancellationToken)
            ?? throw new NotFoundException("ESimPackageEntity Not Found");
        double price = eSimPackage.Price;
        Reattach(eSimPackage, command.Entity, dbContext);
        eSimPackage.Price = price;
        dbContext.Update(eSimPackage);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async virtual Task Delete(DeleteESimPackageCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var eSimPackage = await dbContext.ESimPackages
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw new NotFoundException("ESimPackageEntity Not Found");
        eSimPackage.Status = ContentStatus.Inactive;
        dbContext.Update(eSimPackage);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static DateTime _lastSyncTime = DateTime.MinValue;
    private static readonly SemaphoreSlim _syncLock = new(1, 1);

    public virtual async Task SyncPackages(SyncESimPackagesCommand command, CancellationToken cancellationToken = default)
    {
        await _syncLock.WaitAsync(cancellationToken);
        try
        {
            if ((DateTime.UtcNow - _lastSyncTime) < TimeSpan.FromHours(1))
            {
                Console.WriteLine("SyncPackages skipped due to debounce (less than 1 hour since last execution).");
                return;
            }

            _lastSyncTime = DateTime.UtcNow;

            if (Invalidation.IsActive)
            {
                _ = await Invalidate();
                return;
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);

            string[] types = ["local", "global"];
            foreach(var type in types)
            {
                int pageSize = 20;
                int page = 1;
                bool hasMore = true;
                while(hasMore)
                {
                    var packages = await airaloPackageService.GetAllAsync(new TableOptions()
                    {
                        PageSize = pageSize,
                        Page = page++
                    }, type, cancellationToken);

                    if (packages is null || packages.Data.Count == 0)
                    {
                        hasMore = false; // No more pages
                        continue;
                    }

                    var slugs = packages.Data;
                    var databaseSlugs = await dbContext.ESimSlugs.ToListAsync(cancellationToken);
                    List<ESimPackageEntity> packageViews = [];
                    foreach (var slug in slugs)
                    {
                        var slugEntity = databaseSlugs.FirstOrDefault(x => x.Slug == slug.Slug);
                        if (slugEntity is null)
                        {
                            Console.WriteLine($"ESimSlugEntity with slug {slug.Slug} not found in database.");
                            continue;
                        }

                        if (slug.Operators is null || slug.Operators.Count == 0)
                        {
                            continue; // Skip if no operators
                        }
                        foreach (var operatorPackage in slug.Operators)
                        {
                            List<long> locals = [];
                            if (operatorPackage.Countries.Count > 1)
                            {
                                locals = [.. operatorPackage.Countries.Select(country =>
                                {
                                    var dbSlug = databaseSlugs.FirstOrDefault(x => !string.IsNullOrEmpty(x.CountryCode) &&
                                                                                   x.CountryCode == country.CountryCode &&
                                                                                   x.SlugType == ESimSlugType.Local);
                                    if (dbSlug is null)
                                    {
                                        return 0;
                                    }
                                    return dbSlug.Id;
                                }).Where(x => x != 0)];
                            }
                            foreach (var package in operatorPackage.Packages)
                            {
                                ESimPackageEntity packageView = new()
                                {
                                    PackageId = package.Id,
                                    CountryCode = slug.CountryCode,
                                    CountryName = slug.Title,
                                    DataVolume = $"{package.Amount / 1024} GB",
                                    ValidDays = package.Day,
                                    Price = package.Price,
                                    Network = operatorPackage.Title,
                                    ActivationPolicy = operatorPackage.ActivationPolicy,
                                    OperatorName = operatorPackage.Title,
                                    IsRoaming = operatorPackage.IsRoaming,
                                    ImageUrl = operatorPackage.Image.Url,
                                    Info = operatorPackage.Info,
                                    OtherInfo = operatorPackage.OtherInfo,
                                    Coverage = operatorPackage.Coverages,
                                    ESimSlugId = slugEntity.Id,
                                    Voice = package.Voice ?? 0,
                                    Text = package.Text ?? 0,
                                    HasVoicePack = package.Voice > 0,
                                    Locals = locals
                                };
                                packageViews.Add(packageView);
                            }
                        }
                    }
                    foreach (var package in packageViews)
                    {
                        var existingPackage = await dbContext.ESimPackages
                            .FirstOrDefaultAsync(x => x.PackageId == package.PackageId, cancellationToken);
                        if (existingPackage != null)
                        {
                            if (existingPackage.Price != package.Price)
                            {
                                long id = existingPackage.Id;
                                var status = existingPackage.Status;
                                dbContext.ESimPackages.Update(existingPackage);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            package.Status = ContentStatus.Active;
                            dbContext.ESimPackages.Add(package);
                        }
                    }
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            stopwatch.Stop();
            Console.WriteLine($"SyncPackages completed in {stopwatch.ElapsedMilliseconds} ms");
        }
        finally
        {
            _syncLock.Release();
        }
    }

    public virtual async Task UpdateDiscount(UpdatePackageDiscountCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var eSimPackage = await dbContext.ESimPackages
            .FirstOrDefaultAsync(x => x.Id == command.Entity.Id, cancellationToken)
            ?? throw new NotFoundException("ESimPackageEntity Not Found");

        var packageDiscount = await dbContext.PackageDiscounts
                .FirstOrDefaultAsync(x => x.Id == eSimPackage.PackageDiscountId, cancellationToken);
        if (packageDiscount is null)
        {
            packageDiscount = new()
            {
                ESimPackageId = command.Entity.Id,
                DiscountPercentage = command.Entity.PackageDiscountView!.DiscountPercentage,
                DiscountPrice = command.Entity.PackageDiscountView.DiscountPrice,
                Status = command.Entity.PackageDiscountView.Status,
                StartDate = ConvertToUtc(command.Entity.PackageDiscountView.StartDate),
                EndDate = ConvertToUtc(command.Entity.PackageDiscountView.EndDate),
            };
        }
        else
        {
            packageDiscount.DiscountPercentage = command.Entity.PackageDiscountView!.DiscountPercentage;
            packageDiscount.DiscountPrice = command.Entity.PackageDiscountView.DiscountPrice;
            packageDiscount.Status = command.Entity.PackageDiscountView.Status;
            packageDiscount.StartDate = ConvertToUtc(command.Entity.PackageDiscountView.StartDate);
            packageDiscount.EndDate = ConvertToUtc(command.Entity.PackageDiscountView.EndDate);
        }
        packageDiscount.ESimPackage = null;
        dbContext.Update(packageDiscount);
        await dbContext.SaveChangesAsync(cancellationToken);

        eSimPackage.PackageDiscountId = packageDiscount.Id;
        dbContext.Update(eSimPackage);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<ESimOrderView> MakeOrder(MakeESimOrderCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return new();
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var eSimPackage = await dbContext.ESimPackages
            .FirstOrDefaultAsync(x => x.PackageId == command.PackageId, cancellationToken)
            ?? throw new NotFoundException("ESimPackageEntity Not Found");

        var user = await userService.GetUserAsync(command.Session, cancellationToken)
            ?? throw new NotFoundException("User Not Found");

        double price = eSimPackage.CustomPrice;
        PackageDiscountEntity? discountEntity = null;
        if (eSimPackage.PackageDiscountId > 0)
        {
            discountEntity = await dbContext.PackageDiscounts
                .FirstOrDefaultAsync(x => x.Id == eSimPackage.PackageDiscountId, cancellationToken);
            if (discountEntity is not null &&
                discountEntity.Status == ContentStatus.Active &&
                discountEntity.StartDate <= DateTime.UtcNow &&
                discountEntity.EndDate >= DateTime.UtcNow)
            {
                price = discountEntity.DiscountPrice;
            }
            else
            {
                discountEntity = null;
            }
        }

        IAiraloBalanceService airaloBalanceService = new AiraloBalanceService(airaloTokenService, configuration);
        var balanceCheck = await airaloBalanceService.Get(cancellationToken);
        var currency = await currencyService.GetUsdCourse(cancellationToken);
        double rate = double.Parse(currency.Rate.Replace(",", "."), CultureInfo.InvariantCulture);
        price /= rate;
        if (balanceCheck is null || balanceCheck.Data.Balances.AvailableBalance.Amount < price)
        {
            throw new BadRequestException("Insufficient balance to make an order.");
        }

        InvoiceRequest invoiceRequest = new()
        {
            Amount = (decimal)eSimPackage.CustomPrice,
            Description = "A purchase of eSIM package",
            MerchantId = -777,
        };

        await commander.Call(new CreateInvoiceCommand(command.Session, invoiceRequest), cancellationToken);

        var result = await commander.Call(new OrderArialoPackageCommand(command.PackageId), cancellationToken);
        var order = (ESimOrderView)result;
        order.UserId = user.Id;
        order.CustomPrice = eSimPackage.CustomPrice;
        if (discountEntity is not null)
        {
            order.DiscountPercentage = discountEntity.DiscountPercentage;
        }

        return await commander.Call(new CreateESimOrderCommand(command.Session, order), cancellationToken);
    }
    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
    private void Reattach(ESimPackageEntity eSimPackage, ESimPackageView eSimPackageView, AppDbContext dbContext)
    {
        ESimPackageMapper.From(eSimPackageView, eSimPackage);
    }

    private void Sorting(ref IQueryable<ESimPackageEntity> eSimPackage, TableOptions options) => eSimPackage = options.SortLabel switch
    {
        "PackageId" => eSimPackage.Ordering(options, o => o.PackageId),
        "CountryCode" => eSimPackage.Ordering(options, o => o.CountryCode),
        "CountryName" => eSimPackage.Ordering(options, o => o.CountryName),
        "DataVolume" => eSimPackage.Ordering(options, o => o.DataVolume),
        "ValidDays" => eSimPackage.Ordering(options, o => o.ValidDays),
        "Network" => eSimPackage.Ordering(options, o => o.Network),
        "ActivationPolicy" => eSimPackage.Ordering(options, o => o.ActivationPolicy),
        "Status" => eSimPackage.Ordering(options, o => o.Status),
        "Price" => eSimPackage.Ordering(options, o => o.Price),
        "CustomPrice" => eSimPackage.Ordering(options, o => o.CustomPrice),
        _ => eSimPackage.OrderBy(o => o.Id),

    };

    public static DateTime ConvertToUtc(DateTime? inputDate)
    {
        if (!inputDate.HasValue)
            return DateTime.UtcNow;

        var date = inputDate.Value;

        // Assume Local if unspecified
        if (date.Kind == DateTimeKind.Unspecified)
            date = DateTime.SpecifyKind(date, DateTimeKind.Local);

        return date.ToUniversalTime();
    }

    #endregion
}

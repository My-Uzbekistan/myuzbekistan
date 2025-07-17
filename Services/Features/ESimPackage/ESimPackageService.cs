using System.Diagnostics;

namespace myuzbekistan.Services;

public class ESimPackageService(
    IServiceProvider services, 
    IAiraloCountryService airaloCountryService,
    IAiraloPackageService airaloPackageService,
    ICurrencyService currencyService,
    IUserService userService) 
    : DbServiceBase<AppDbContext>(services), IESimPackageService
{
    #region Queries

    public async virtual Task<TableResponse<ESimPackageView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var eSimPackage = from s in dbContext.ESimPackages select s;

        if (!string.IsNullOrEmpty(options.Search))
        {
            eSimPackage = eSimPackage.Where(s =>
                     s.PackageId.Contains(options.Search)
            );
        }

        Sorting(ref eSimPackage, options);

        var count = await eSimPackage.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await eSimPackage.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        var views = items.MapToViewList();
        var currency = await currencyService.GetUsdCourse(cancellationToken);
        double rate = double.Parse(currency.Rate);
        foreach (var view in views)
        {
            view.Price = view.Price * rate;
            if (view.PackageDiscountId.HasValue)
            {
                var packageDiscount = await dbContext.PackageDiscounts
                    .FirstOrDefaultAsync(x => x.Id == view.PackageDiscountId, cancellationToken);
                if (packageDiscount != null)
                {
                    view.PackageDiscountView = packageDiscount.MapToView();
                }
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
            ?? throw new ValidationException("ESimPackageEntity Not Found");

        var view = eSimPackage.MapToView();
        var currency = await currencyService.GetUsdCourse(cancellationToken);
        double rate = double.Parse(currency.Rate);
        view.Price = view.Price * rate;
        return view;
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
        dbContext.Add(eSimPackage);
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
            ?? throw new ValidationException("ESimPackageEntity Not Found");
        eSimPackage.Status = ContentStatus.Inactive;
        dbContext.Update(eSimPackage);
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
            ?? throw new ValidationException("ESimPackageEntity Not Found");
        double price = eSimPackage.Price;
        Reattach(eSimPackage, command.Entity, dbContext);
        eSimPackage.Price = price;
        dbContext.Update(eSimPackage);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task SyncPackages(SyncESimPackagesCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        Stopwatch stopwatch = Stopwatch.StartNew();
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);

        var countries = await airaloCountryService.GetAllAsync(Language.en, cancellationToken);
        foreach (var country in countries)
        {
            var packages = await airaloPackageService.GetCountryPackagesAsync(country.Slug, cancellationToken);
            var esimPackages = ESimPackageView.FromApiResponse(packages);
            foreach (var package in esimPackages)
            {
                var existingPackage = await dbContext.ESimPackages
                    .FirstOrDefaultAsync(x => x.PackageId == package.PackageId && x.CountryCode == package.CountryCode, cancellationToken);
                if (existingPackage != null && existingPackage.Price != package.Price)
                {
                    long id = existingPackage.Id;
                    var status = existingPackage.Status;
                    Reattach(existingPackage, package, dbContext);
                    existingPackage.Id = id;
                    existingPackage.Status = status;
                    dbContext.Update(existingPackage);
                }
                else
                {
                    ESimPackageEntity eSimPackage = new ESimPackageEntity();
                    Reattach(eSimPackage, package, dbContext);
                    dbContext.Add(eSimPackage);
                }
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        stopwatch.Stop();
        Console.WriteLine($"SyncPackages completed in {stopwatch.ElapsedMilliseconds} ms");
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
                DiscountPercentage = command.Entity.PackageDiscountView.DiscountPercentage,
                DiscountPrice = command.Entity.PackageDiscountView.DiscountPrice,
                Status = command.Entity.PackageDiscountView.Status,
                StartDate = ConvertToUtc(command.Entity.PackageDiscountView.StartDate),
                EndDate = ConvertToUtc(command.Entity.PackageDiscountView.EndDate),
            };
        }
        else
        {
            packageDiscount.DiscountPercentage = command.Entity.PackageDiscountView.DiscountPercentage;
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

    public virtual async Task MakeOrder(MakeESimOrderCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var eSimPackage = await dbContext.ESimPackages
            .FirstOrDefaultAsync(x => x.PackageId == command.PackageId, cancellationToken)
            ?? throw new NotFoundException("ESimPackageEntity Not Found");

        var user = await userService.GetUserAsync(command.Session, cancellationToken)
            ?? throw new NotFoundException("User Not Found");

        ESimOrderEntity eSimOrder = new()
        {

        }
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

using System.Diagnostics;

namespace myuzbekistan.Services;

public class ESimSlugService(IServiceProvider services,
    IAiraloCountryService airaloCountryService) : DbServiceBase<AppDbContext>(services), IESimSlugService
{
    #region Queries

    public async virtual Task<TableResponse<ESimSlugView>> GetAllCountries(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var esimSlug = from s in dbContext.ESimSlugs select s;
        esimSlug = esimSlug.Where(s => s.SlugType == ESimSlugType.Local);

        if (!string.IsNullOrEmpty(options.Search))
        {
            esimSlug = esimSlug.Where(x => x.TitleUz.Contains(options.Search)
                || x.TitleRu.Contains(options.Search)
                || x.TitleEn.Contains(options.Search));
        }

        var count = await esimSlug.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await esimSlug.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        Language language = options.Lang.ConvertToLanguage();
        return new()
        {
            TotalItems = count,
            Items = [.. items.Select(x => x.ToView(language))]
        };
    }

    public async virtual Task<List<ESimSlugView>> GetAllPopularCountries(Language language, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        var popularCountries = await airaloCountryService.GetPopularAsync(language, cancellationToken);
        List<string> slugs = popularCountries.Select(x => x.Slug).ToList();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var esimSlug = from s in dbContext.ESimSlugs select s;
        esimSlug = esimSlug.Where(s => s.SlugType == ESimSlugType.Local);
        esimSlug = esimSlug.Where(x => slugs.Any(y => y == x.Slug));
        var count = await esimSlug.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await esimSlug.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
        return [.. items.Select(x => x.ToView(language))];
    }

    public async virtual Task<List<ESimSlugView>> GetAllRegions(Language language, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var esimSlug = from s in dbContext.ESimSlugs select s;

        esimSlug = esimSlug.Where(s => s.SlugType == ESimSlugType.Regional);

        var count = await esimSlug.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await esimSlug.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
        return [.. items.Select(x => x.ToView(language))];
    }

    public async virtual Task<ESimSlugView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var esimSlug = await dbContext.ESimSlugs
            .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken)
            ?? throw new NotFoundException("ESimSlugEntity Not Found");

        return esimSlug.MapToView();
    }

    #endregion

    #region Mutations

    public async virtual Task Sync(SyncESimSlugCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);

        #region Regional slugs
        Stopwatch stopwatch = Stopwatch.StartNew();
        List<ESimSlugEntity> globalSlugs = [];
        var regionsUzTask = airaloCountryService.GetRegionsAsync(Language.uz, cancellationToken);
        var regionsRuTask = airaloCountryService.GetRegionsAsync(Language.ru, cancellationToken);
        var regionsEnTask = airaloCountryService.GetRegionsAsync(Language.en, cancellationToken);
        await Task.WhenAll(regionsUzTask, regionsRuTask, regionsEnTask);
        var regionsUz = regionsUzTask.Result;
        var regionsRu = regionsRuTask.Result;
        var regionsEn = regionsEnTask.Result;

        foreach (var region in regionsUz)
        {
            var slug = new ESimSlugEntity
            {
                SlugType = ESimSlugType.Regional,
                Slug = region.Slug,
                TitleUz = region.Title,
                TitleRu = regionsRu.FirstOrDefault(x => x.Slug == region.Slug)?.Title ?? string.Empty,
                TitleEn = regionsEn.FirstOrDefault(x => x.Slug == region.Slug)?.Title ?? string.Empty,
                ImageUrl = region.Image.Url,
                CountryCode = null
            };
            var existingSlug = await dbContext.ESimSlugs
                .FirstOrDefaultAsync(x => x.Slug == slug.Slug && x.SlugType == slug.SlugType, cancellationToken);
            if (existingSlug is null)
            {
                globalSlugs.Add(slug);
            }
        }
        dbContext.ESimSlugs.AddRange(globalSlugs);
        await dbContext.SaveChangesAsync(cancellationToken);
        stopwatch.Stop();
        Console.WriteLine($"{globalSlugs.Count} Regions fetched in {stopwatch.ElapsedMilliseconds} ms");
        #endregion

        #region Local Slugs
        stopwatch.Restart();
        List<ESimSlugEntity> countrySlugs = [];
        var countriesUzTask = airaloCountryService.GetAllAsync(Language.uz, cancellationToken);
        var countriesRuTask = airaloCountryService.GetAllAsync(Language.ru, cancellationToken);
        var countriesEnTask = airaloCountryService.GetAllAsync(Language.en, cancellationToken);
        await Task.WhenAll(countriesUzTask, countriesRuTask, countriesEnTask);
        var countriesUz = countriesUzTask.Result;
        var countriesRu = countriesRuTask.Result;
        var countriesEn = countriesEnTask.Result;
        foreach (var country in countriesUz)
        {
            CountryCodes.Dictionary.TryGetValue(country.Slug, out var countryCode);
            var slug = new ESimSlugEntity
            {
                SlugType = ESimSlugType.Local,
                Slug = country.Slug,
                TitleUz = country.Title,
                TitleRu = countriesRu.FirstOrDefault(x => x.Slug == country.Slug)?.Title ?? string.Empty,
                TitleEn = countriesEn.FirstOrDefault(x => x.Slug == country.Slug)?.Title ?? string.Empty,
                ImageUrl = country.Image.Url,
                CountryCode = countryCode
            };
            var existingSlug = await dbContext.ESimSlugs
                .FirstOrDefaultAsync(x => x.Slug == slug.Slug && x.SlugType == slug.SlugType, cancellationToken);
            if (existingSlug is null)
            {
                countrySlugs.Add(slug);
            }
        }
        dbContext.ESimSlugs.AddRange(countrySlugs);
        await dbContext.SaveChangesAsync(cancellationToken);
        stopwatch.Stop();
        Console.WriteLine($"{countrySlugs.Count} Countries fetched in {stopwatch.ElapsedMilliseconds} ms");
        #endregion

        #region Global Slug
        ESimSlugEntity worldSlug = new()
        {
            TitleEn = "World",
            TitleUz = "Dunyo",
            TitleRu = "Мир",
            Slug = "world",
            ImageUrl = "https://sandbox.airalo.com/images/1bf385c8-c178-4797-9482-faee55e13c36.png",
            SlugType = ESimSlugType.Global
        };
        var existingWorldSlug = await dbContext.ESimSlugs
            .FirstOrDefaultAsync(x => x.Slug == worldSlug.Slug && x.SlugType == worldSlug.SlugType, cancellationToken);
        if (existingWorldSlug is null)
        {
            dbContext.ESimSlugs.Add(worldSlug);
            await dbContext.SaveChangesAsync(cancellationToken);
            Console.WriteLine($"World slug added");
        } 
        #endregion
    }

    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    private static void Reattach(ESimSlugEntity esimSlug, ESimSlugView esimSlugView, AppDbContext dbContext)
    {
        ESimSlugMapper.From(esimSlugView, esimSlug);

    }
    #endregion
}

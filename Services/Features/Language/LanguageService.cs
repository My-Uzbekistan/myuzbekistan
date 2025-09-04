using Microsoft.EntityFrameworkCore;
using ActualLab.Fusion;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using ActualLab.Async;
using System.Reactive;
namespace myuzbekistan.Services;

public class LanguageService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), ILanguageService 
{
    #region Queries
    [ComputeMethod]
    public async virtual Task<TableResponse<LanguageView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var language = from s in dbContext.Languages select s;

        if (!String.IsNullOrEmpty(options.Search))
        {
            language = language.Where(s => 
                     s.Name.Contains(options.Search)
                    || s.Locale.Contains(options.Search)
            );
        }

        Sorting(ref language, options);

        if (!String.IsNullOrEmpty(options.Lang))
            language = language.Where(x => x.Locale.Equals(options.Lang));

        var count = await language.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await language.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<LanguageView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<List<LanguageView>> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var language = dbContext.Languages
        .Where(x => x.Id == Id).ToList();
        
        return language == null ? throw new NotFoundException("LanguageEntity Not Found") : language.MapToViewList();
    }

    #endregion
    #region Mutations
    long maxId;

    public async virtual Task Create(CreateLanguageCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        maxId = !dbContext.Languages.Any() ? 0 : dbContext.Languages.Max(x => x.Id);
        maxId++;
        foreach (var item in command.Entity)
        {
            LanguageEntity language = new LanguageEntity();
            Reattach(language, item, dbContext);
            language.Id = maxId;
            dbContext.Add(language);

        }
        await dbContext.SaveChangesAsync(cancellationToken);

    }


    public async virtual Task Delete(DeleteLanguageCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var language = await dbContext.Languages
            .Where(x => x.Id == command.Id)
        .ToListAsync(cancellationToken: cancellationToken) ?? throw new ValidationException("LanguageEntity Not Found");
        dbContext.RemoveRange(language);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task Update(UpdateLanguageCommand command, CancellationToken cancellationToken = default)
    {
        var lang = command.Entity.First();
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var language =  dbContext.Languages
        .Where(x => x.Id == lang.Id).AsNoTracking().ToList();

        if (language == null) throw  new ValidationException("LanguageEntity Not Found");

        foreach (var item in command.Entity)
        {
            Reattach(language.First(x => x.Locale == item.Locale), item, dbContext);
            dbContext.Update(language.First(x => x.Locale == item.Locale));
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
    private void Reattach(LanguageEntity language, LanguageView languageView, AppDbContext dbContext)
    {
        LanguageMapper.From(languageView, language);



    }

    private void Sorting(ref IQueryable<LanguageEntity> language, TableOptions options) => language = options.SortLabel switch
    {
        "Name" => language.Ordering(options, o => o.Name),
        "Locale" => language.Ordering(options, o => o.Locale),
        "Id" => language.Ordering(options, o => o.Id),
        _ => language.OrderBy(o => o.Id),
        
    };
    #endregion
}

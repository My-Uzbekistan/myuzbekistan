using myuzbekistan.Services;
using myuzbekistan.Shared;

public class SmsTemplateService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), ISmsTemplateService 
{
    #region Queries

    [ComputeMethod]
    public async virtual Task<TableResponse<SmsTemplateView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var smstemplate = from s in dbContext.SmsTemplates select s;

        if (!string.IsNullOrEmpty(options.Search))
        {
            smstemplate = smstemplate.Where(s => 
                     s.Locale.Contains(options.Search)
                    || s.Template.Contains(options.Search)
                    || s.Key.Contains(options.Search)
            );
        }

        Sorting(ref smstemplate, options);

        if (!String.IsNullOrEmpty(options.Lang))
            smstemplate = smstemplate.Where(x => x.Locale.Equals(options.Lang));

        var count = await smstemplate.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await smstemplate.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<SmsTemplateView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<List<SmsTemplateView>> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var smstemplate = await dbContext.SmsTemplates
            .Where(x => x.Id == Id)
            .ToListAsync(cancellationToken)
            ?? throw  new ValidationException("SmsTemplateEntity Not Found");
        
        return smstemplate.MapToViewList();
    }

    #endregion

    #region Mutations
    long maxId;
    public async virtual Task Create(CreateSmsTemplateCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        maxId = !dbContext.SmsTemplates.Any() ? 0 : dbContext.SmsTemplates.Max(x => x.Id);
        maxId++;
        foreach (var item in command.Entity)
        {
            SmsTemplateEntity language = new SmsTemplateEntity();
            Reattach(language, item, dbContext);
            language.Id = maxId;
            dbContext.Add(language);

        }
        await dbContext.SaveChangesAsync(cancellationToken);

    }

    public async virtual Task Update(UpdateSmsTemplateCommand command, CancellationToken cancellationToken = default)
    {
        var temp = command.Entity.First();
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var smsTemplate = dbContext.SmsTemplates
        .Where(x => x.Id == temp.Id).AsNoTracking().ToList();

        if (smsTemplate == null) throw new ValidationException("LanguageEntity Not Found");

        foreach (var item in command.Entity)
        {
            Reattach(smsTemplate.First(x => x.Locale == item.Locale), item, dbContext);
            dbContext.Update(smsTemplate.First(x => x.Locale == item.Locale));
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async virtual Task Delete(DeleteSmsTemplateCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var language = await dbContext.SmsTemplates
            .Where(x => x.Id == command.Id)
        .ToListAsync(cancellationToken: cancellationToken) ?? throw new ValidationException("LanguageEntity Not Found");
        dbContext.RemoveRange(language);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion


    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    private static void Reattach(SmsTemplateEntity smstemplate, SmsTemplateView smstemplateView, AppDbContext dbContext)
    {
        SmsTemplateMapper.From(smstemplateView, smstemplate);

    }

    private static void Sorting(ref IQueryable<SmsTemplateEntity> smstemplate, TableOptions options) 
        => smstemplate = options.SortLabel switch
        {
            "Locale" => smstemplate.Ordering(options, o => o.Locale),
            "Template" => smstemplate.Ordering(options, o => o.Template),
            "Key" => smstemplate.Ordering(options, o => o.Key),
            "Id" => smstemplate.Ordering(options, o => o.Id),
            _ => smstemplate.OrderBy(o => o.Id),
        
        };

    #endregion
}

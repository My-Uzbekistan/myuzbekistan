using ActualLab.Async;
using ActualLab.Fusion;
using ActualLab.Fusion.EntityFramework;
using Microsoft.EntityFrameworkCore;
using myuzbekistan.Services;
using myuzbekistan.Shared;
using System.ComponentModel.DataAnnotations;
using System.Reactive;

public class MerchantService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), IMerchantService 
{
    #region Queries

    [ComputeMethod]
    public async virtual Task<TableResponse<MerchantView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var merchant = from s in dbContext.Merchants select s;

        if (!string.IsNullOrEmpty(options.Search))
        {
            merchant = merchant.Where(s => 
                     s.BrandName !=null && s.BrandName.Contains(options.Search)
                    || s.Name !=null && s.Name.Contains(options.Search)
                    || s.Phone !=null && s.Phone.Contains(options.Search)
                    || s.Email !=null && s.Email.Contains(options.Search)
                    || s.Address !=null && s.Address.Contains(options.Search)
                    || s.Description !=null && s.Description.Contains(options.Search)
                    || s.Contract !=null && s.Contract.Contains(options.Search)
                    || s.Inn.Contains(options.Search)
                    || s.Mfi !=null && s.Mfi.Contains(options.Search)
                    || s.AccountNumber.Contains(options.Search)
                    || s.Responsible.Contains(options.Search)
                    || s.TypeOfService.Contains(options.Search)
                    || s.MXIKCode.Contains(options.Search)
            );
        }

        Sorting(ref merchant, options);
        
        merchant = merchant.Include(x => x.Image);
        merchant = merchant.Include(x => x.Parent);
        var count = await merchant.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await merchant.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<MerchantView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<MerchantView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var merchant = await dbContext.Merchants
            .Include(x => x.Image)
            .Include(x => x.Parent)
            .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken)
            ?? throw  new ValidationException("MerchantEntity Not Found");
        
        return merchant.MapToView();
    }

    #endregion

    #region Mutations

    public async virtual Task Create(CreateMerchantCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        MerchantEntity merchant = new();
        Reattach(merchant, command.Entity, dbContext); 
        
        dbContext.Update(merchant);
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
        var merchant = await dbContext.Merchants
            .Include(x=>x.Image)
            .Include(x=>x.Parent)
            .FirstOrDefaultAsync(x => x.Id == command.Entity.Id, cancellationToken)
            ?? throw  new ValidationException("MerchantEntity Not Found");

        Reattach(merchant, command.Entity, dbContext);
        
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
        var merchant = await dbContext.Merchants
            .Include(x=>x.Image)
            .Include(x=>x.Parent)
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw  new ValidationException("MerchantEntity Not Found");
        dbContext.Remove(merchant);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    private static void Reattach(MerchantEntity merchant, MerchantView merchantView, AppDbContext dbContext)
    {
        MerchantMapper.From(merchantView, merchant);

        if(merchant.Image != null)
        merchant.Image = dbContext.Files
        .First(x => x.Id == merchant.Image.Id);
        if(merchant.Parent != null)
        merchant.Parent = dbContext.Merchants
        .First(x => x.Id == merchant.Parent.Id);
    }

    private static void Sorting(ref IQueryable<MerchantEntity> merchant, TableOptions options) 
        => merchant = options.SortLabel switch
        {
            "BrandName" => merchant.Ordering(options, o => o.BrandName),
            "Name" => merchant.Ordering(options, o => o.Name),
            "Phone" => merchant.Ordering(options, o => o.Phone),
            "Email" => merchant.Ordering(options, o => o.Email),
            "Address" => merchant.Ordering(options, o => o.Address),
            "Description" => merchant.Ordering(options, o => o.Description),
            "Contract" => merchant.Ordering(options, o => o.Contract),
            "Inn" => merchant.Ordering(options, o => o.Inn),
            "Mfi" => merchant.Ordering(options, o => o.Mfi),
            "AccountNumber" => merchant.Ordering(options, o => o.AccountNumber),
            "Discount" => merchant.Ordering(options, o => o.Discount),
            "IsVat" => merchant.Ordering(options, o => o.IsVat),
            "Image" => merchant.Ordering(options, o => o.Image),
            "IsActive" => merchant.Ordering(options, o => o.IsActive),
            "Parent" => merchant.Ordering(options, o => o.Parent),
            "PayDay" => merchant.Ordering(options, o => o.PayDay),
            "Responsible" => merchant.Ordering(options, o => o.Responsible),
            "TypeOfService" => merchant.Ordering(options, o => o.TypeOfService),
            "MXIKCode" => merchant.Ordering(options, o => o.MXIKCode),
            "Id" => merchant.Ordering(options, o => o.Id),
            _ => merchant.OrderBy(o => o.Id),
        
        };

    #endregion
}

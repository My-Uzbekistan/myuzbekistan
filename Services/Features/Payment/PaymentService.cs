using Microsoft.EntityFrameworkCore;
using ActualLab.Fusion;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using ActualLab.Async;
using System.Reactive;
namespace myuzbekistan.Services;

public class PaymentService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), IPaymentService 
{
    #region Queries
    [ComputeMethod]
    public async virtual Task<TableResponse<PaymentView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var payment = from s in dbContext.Payments select s;

        if (!String.IsNullOrEmpty(options.Search))
        {
            payment = payment.Where(s => 
                     s.PaymentMethod !=null && s.PaymentMethod.Contains(options.Search)
                    || s.TransactionId !=null && s.TransactionId.Contains(options.Search)
                    || s.CallbackData !=null && s.CallbackData.Contains(options.Search)
            );
        }

        Sorting(ref payment, options);
        
        var count = await payment.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await payment.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<PaymentView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<PaymentView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var payment = await dbContext.Payments
        .FirstOrDefaultAsync(x => x.Id == Id);
        
        return payment == null ? throw new NotFoundException("PaymentEntity Not Found") : payment.MapToView();
    }

    public async virtual Task<PaymentView> GetByExternalId(string externalId, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var payment = await dbContext.Payments
        .FirstOrDefaultAsync(x => x.ExternalId == externalId);

        return payment == null ? throw new NotFoundException("PaymentEntity Not Found") : payment.MapToView();
    }


    [ComputeMethod]
    public async virtual Task<PaymentView> GetByTransactionId(string Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var payment = await dbContext.Payments
        .FirstOrDefaultAsync(x => x.TransactionId == Id);
        
        return payment == null ? throw new NotFoundException("PaymentEntity Not Found") : payment.MapToView();
    }

    #endregion
    #region Mutations
    public async virtual Task Create(CreatePaymentCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        PaymentEntity payment=new PaymentEntity();
        Reattach(payment, command.Entity, dbContext); 
        
        dbContext.Update(payment);
        await dbContext.SaveChangesAsync(cancellationToken);

    }


    public async virtual Task Delete(DeletePaymentCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var payment = await dbContext.Payments
        .FirstOrDefaultAsync(x => x.Id == command.Id);
        if (payment == null) throw  new ValidationException("PaymentEntity Not Found");
        dbContext.Remove(payment);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task Update(UpdatePaymentCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var payment = await dbContext.Payments
        .FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

        if (payment == null) throw  new ValidationException("PaymentEntity Not Found"); 

        Reattach(payment, command.Entity, dbContext);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
    private void Reattach(PaymentEntity payment, PaymentView paymentView, AppDbContext dbContext)
    {
        PaymentMapper.From(paymentView, payment);



    }

    private void Sorting(ref IQueryable<PaymentEntity> payment, TableOptions options) => payment = options.SortLabel switch
    {
        "UserId" => payment.Ordering(options, o => o.UserId),
        "PaymentMethod" => payment.Ordering(options, o => o.PaymentMethod),
        "Amount" => payment.Ordering(options, o => o.Amount),
        "PaymentStatus" => payment.Ordering(options, o => o.PaymentStatus),
        "TransactionId" => payment.Ordering(options, o => o.TransactionId),
        "CallbackData" => payment.Ordering(options, o => o.CallbackData),
        "Id" => payment.Ordering(options, o => o.Id),
        _ => payment.OrderBy(o => o.Id),
        
    };
    #endregion
}

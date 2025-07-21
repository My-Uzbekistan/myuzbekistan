using Microsoft.EntityFrameworkCore.Internal;
using myuzbekistan.Services;

public class InvoiceService(IServiceProvider services, IAuth auth, IDbContextFactory<ApplicationDbContext> dbContextFactory, MerchantNotifierService merchantNotifier) : DbServiceBase<AppDbContext>(services), IInvoiceService
{
    #region Queries

    [ComputeMethod]
    public async virtual Task<TableResponse<InvoiceView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var invoice = from s in dbContext.Invoices select s;

        if (!string.IsNullOrEmpty(options.Search))
        {
            invoice = invoice.Where(s =>
                     s.Currency != null && s.Currency.Contains(options.Search)
                    || s.Description != null && s.Description.Contains(options.Search)
            );
        }

        Sorting(ref invoice, options);

        invoice = invoice.Include(x => x.Merchant);
        var count = await invoice.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await invoice.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<InvoiceView>() { Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<InvoiceView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var invoice = await dbContext.Invoices
            .Include(x => x.Merchant)
            .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken)
            ?? throw new NotFoundException("InvoiceEntity Not Found");

        return invoice.MapToView();
    }

    #endregion

    #region Mutations

    public async virtual Task Create(CreateInvoiceCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        await using var userContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var userSession = await auth.GetUser(command.Session, cancellationToken);
        long userId = long.Parse(userSession!.Claims.First(x => x.Key.Equals(ClaimTypes.NameIdentifier)).Value);
        var user = userContext.Users.FirstOrDefault(x => x.Id == userId)
            ?? throw new NotFoundException("User Not Found");


        if (user.Balance < command.InvoiceRequest.Amount)
            throw new ValidationException("Insufficient balance to create invoice.");


        var merchant = dbContext.Merchants.Include(x => x.MerchantCategory).First(x => x.Id == command.InvoiceRequest.MerchantId);
        InvoiceEntity invoice = new();
        invoice.Merchant = merchant;
        invoice.User = user;
        invoice.UserId = user.Id;
        invoice.Amount = command.InvoiceRequest.Amount;
        invoice.Description = command.InvoiceRequest.Description;
        user.Balance -= command.InvoiceRequest.Amount;
        dbContext.Update(invoice);
        await userContext.SaveChangesAsync();
        await dbContext.SaveChangesAsync(cancellationToken);
        var msg = $"""
        ✅ *Оплата подтверждена*  
        *Место:* {merchant.Name}
        *Сумма:* {invoice.Amount:N2} UZS  
        *Id Транзакции:* {user.FullName} (ID {invoice.Id})  
        *Назначение:* {invoice.Description ?? "—"}  
        *Время:* {DateTime.Now:dd.MM.yyyy HH:mm:ss}
        """;

        await merchantNotifier.Notify(merchant.MerchantCategory.Id, msg, cancellationToken);

    }

    public async virtual Task Update(UpdateInvoiceCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var invoice = await dbContext.Invoices
            .Include(x => x.Merchant)
            .FirstOrDefaultAsync(x => x.Id == command.Entity.Id, cancellationToken)
            ?? throw new NotFoundException("InvoiceEntity Not Found");

        Reattach(invoice, command.Entity, dbContext);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async virtual Task Delete(DeleteInvoiceCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var invoice = await dbContext.Invoices
            .Include(x => x.Merchant)
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw new NotFoundException("InvoiceEntity Not Found");
        dbContext.Remove(invoice);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    private static void Reattach(InvoiceEntity invoice, InvoiceView invoiceView, AppDbContext dbContext)
    {
        InvoiceMapper.From(invoiceView, invoice);

        if (invoice.Merchant != null)
            invoice.Merchant = dbContext.Merchants
            .First(x => x.Id == invoice.Merchant.Id);
    }

    private static void Sorting(ref IQueryable<InvoiceEntity> invoice, TableOptions options)
        => invoice = options.SortLabel switch
        {
            "Amount" => invoice.Ordering(options, o => o.Amount),
            "Currency" => invoice.Ordering(options, o => o.Currency),
            "Description" => invoice.Ordering(options, o => o.Description),
            "Id" => invoice.Ordering(options, o => o.Id),
            _ => invoice.OrderBy(o => o.Id),

        };

    #endregion
}

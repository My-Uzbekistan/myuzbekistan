using myuzbekistan.Services;
using myuzbekistan.Shared;
using System;
using System.Buffers.Text;

public class InvoiceService(IServiceProvider services,
    IAuth auth,
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    MerchantNotifierService merchantNotifier,
    IUserService userService) : DbServiceBase<AppDbContext>(services), IInvoiceService
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

    [ComputeMethod]
    public async virtual Task<InvoiceDetailView> GetByPaymentId(string ExternalId, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var invoice = await dbContext.Invoices
            .Include(x => x.Merchant)
                .ThenInclude(x => x.MerchantCategory).ThenInclude(x => x.ServiceType)
                .Include(x => x.Merchant)
                    .ThenInclude(x => x.Logo)

            .FirstOrDefaultAsync(x => x.ExternalId == ExternalId, cancellationToken)
            ?? throw new NotFoundException("InvoiceEntity Not Found");


        return invoice.MapToDetail();
    }

    [ComputeMethod]
    public async virtual Task<TableResponse<InvoiceSummaryView>> GetByPayments(TableOptions options, long userId, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var invoiceQuery = dbContext.Invoices
            .Include(x => x.Merchant)
                .ThenInclude(x => x.MerchantCategory).ThenInclude(x => x.ServiceType)
                .Include(x => x.Merchant)
                    .ThenInclude(x => x.Logo)

            .Where(x => x.UserId == userId )
            .OrderByDescending(x => x.CreatedAt);

        var count = await invoiceQuery.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await invoiceQuery.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);

        return new TableResponse<InvoiceSummaryView>() { Items = items.MapToSummaryList(), TotalItems = count };
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
        var user = await userService.GetUserAsync(command.Session, cancellationToken)
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
        invoice.ExternalId = command.InvoiceRequest.PaymentId;
        invoice.Status = command.InvoiceRequest.PaymentStatus;
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

namespace myuzbekistan.Services;

public class CardService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), ICardService
{
    #region Queries
    [ComputeMethod]
    public async virtual Task<TableResponse<CardView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var card = from s in dbContext.Cards select s;

        if (!String.IsNullOrEmpty(options.Search))
        {
            card = card.Where(s =>
                     s.ExpirationDate != null && s.ExpirationDate.Contains(options.Search)
                    || s.CardPan.Contains(options.Search)
                    || s.CardToken.Contains(options.Search)
                    || s.Phone.Contains(options.Search)
                    || s.HolderName.Contains(options.Search)
                    || s.Pinfl != null && s.Pinfl.Contains(options.Search)
                    || s.Ps.Contains(options.Search)
                    || s.Status.Contains(options.Search)
            );
        }

        Sorting(ref card, options);

        var count = await card.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await card.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<CardView>() { Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public virtual async Task<List<CardInfo>> GetCardByUserId(long userId, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var card = from s in dbContext.Cards
                   where s.Status == "active" && s.UserId == userId
                   select s;

        return card.ToList().MapToListInfo();
    }

    public async virtual Task<CardView?> GetByPan(long userId, string pan, CancellationToken cancellationToken = default)
    {
        // ── 1. Валидация --------------------------------------------------------
        if (string.IsNullOrWhiteSpace(pan) || !pan.All(char.IsDigit) || pan.Length < 13)
            return null; // невалидный номер

        // ── 2. Маскируем: 6 + * + 4 -------------------------------------------
        string masked = MaskPan(pan);                          // 561468******5173

        // ── 3. Проверяем в БД --------------------------------------------------
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);

        var card = await dbContext.Cards
            .AsNoTracking()
            .FirstOrDefaultAsync(c =>
                c.UserId == userId &&
                c.CardPan == masked,                               // точное совпадение
                cancellationToken);

        return card?.MapToView(); // Возвращаем ID карты или null, если карта не найдена
    }

    /// <summary>
    /// «Экранирует» (маскирует) PAN: первые 6, последние 4,
    /// остальное заменяется на '*'.
    /// </summary>
    private static string MaskPan(string pan)
    {
        int len = pan.Length;
        int stars = len - 10;                                  // сколько '*'
        return pan[..6] + new string('*', stars) + pan[^4..];
    }

    [ComputeMethod]
    public async virtual Task<CardView> Get(long Id, long userId, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var card = await dbContext.Cards
        .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == Id);

        return card == null ? throw new NotFoundException("CardEntity Not Found") : card.MapToView();
    }

    #endregion

    #region Mutations
    public async virtual Task<long> Create(CreateCardCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return default;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        CardEntity card = new CardEntity();
        Reattach(card, command.Entity, dbContext);

        dbContext.Update(card);
        await dbContext.SaveChangesAsync(cancellationToken);
        return card.Id;
    }


    public virtual async Task Delete(DeleteCardCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var card = await dbContext.Cards
        .FirstOrDefaultAsync(x => x.Id == command.Id && x.UserId == command.UserId,
            cancellationToken: cancellationToken) ?? throw new ValidationException("CardEntity Not Found");
        dbContext.Remove(card);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task Update(UpdateCardCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var card = await dbContext.Cards
        .FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

        if (card == null) throw new ValidationException("CardEntity Not Found");

        Reattach(card, command.Entity, dbContext);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region External
    private static string token;

    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
    private void Reattach(CardEntity card, CardView cardView, AppDbContext dbContext)
    {
        CardMapper.From(cardView, card);
    }

    private void Sorting(ref IQueryable<CardEntity> card, TableOptions options) => card = options.SortLabel switch
    {
        "CardId" => card.Ordering(options, o => o.CardId),
        "UserId" => card.Ordering(options, o => o.UserId),
        "ExpirationDate" => card.Ordering(options, o => o.ExpirationDate),
        "CardPan" => card.Ordering(options, o => o.CardPan),
        "CardToken" => card.Ordering(options, o => o.CardToken),
        "Phone" => card.Ordering(options, o => o.Phone),
        "HolderName" => card.Ordering(options, o => o.HolderName),
        "Pinfl" => card.Ordering(options, o => o.Pinfl),
        "Ps" => card.Ordering(options, o => o.Ps),
        "Status" => card.Ordering(options, o => o.Status),
        "Id" => card.Ordering(options, o => o.Id),
        _ => card.OrderBy(o => o.Id),

    };
    #endregion
}
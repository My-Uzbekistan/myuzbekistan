using Microsoft.EntityFrameworkCore;
using myuzbekistan.Services;


public class Datum
{
    public string id { get; set; }
    public string bin { get; set; }
    public string brand { get; set; }
    public string bank { get; set; }
    public string type { get; set; }
    public string cat { get; set; }
    public string country { get; set; }
    public string country_2 { get; set; }
    public string country_3 { get; set; }
    public string iso_code { get; set; }
}

public class BinCheckerApiResponse
{
    public string status { get; set; }
    public List<Datum> data { get; set; }
}

public class CardPrefixService(IServiceProvider services, ILogger<CardPrefixService> logger, ICommander commander) : DbServiceBase<AppDbContext>(services), ICardPrefixService
{
    #region Queries

    [ComputeMethod]
    public async virtual Task<TableResponse<CardPrefixView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var cardprefix = from s in dbContext.CardPrefixes select s;

        if (!string.IsNullOrEmpty(options.Search))
        {
            cardprefix = cardprefix.Where(s =>
                     s.BankName != null && s.BankName.Contains(options.Search)
                    || s.CardType != null && s.CardType.Contains(options.Search)
            );
        }

        Sorting(ref cardprefix, options);

        cardprefix = cardprefix.Include(x => x.CardBrand);
        var count = await cardprefix.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await cardprefix.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<CardPrefixView>() { Items = items.MapToViewList(), TotalItems = count };
    }

    public async Task<CardPrefixApi> GetTypeByCardNumber(string cardNumber, CancellationToken cancellationToken = default)
    {
        if(cardNumber.Count() > 6)
        cardNumber = cardNumber.Substring(0, 6);
        if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 6)
            throw new ValidationException("Card Number must be greater then 6 symbols");

        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);

        var prefix = await dbContext.CardPrefixes
            .FromSqlInterpolated($@"
            SELECT *
            FROM   ""CardPrefixes""
            WHERE  {cardNumber} LIKE (""Prefix""::text || '%')   -- точное «начало строки»
            ORDER  BY length(""Prefix""::text) DESC               -- самый длинный префикс
            LIMIT  1")
            .AsNoTracking()
            .Select(p => new CardPrefixApi
            {
                Prefix = p.Prefix,
                BankName = p.BankName,
                CardBrand = p.CardBrand != null ? p.CardBrand.Path : null,
                CardType = p.CardType
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (prefix == null)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.binchecker.ai/check");
            var content = new StringContent($$"""{"bin":"{{cardNumber}}"}""", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseData = JsonSerializer.Deserialize<BinCheckerApiResponse>(await response.Content.ReadAsStringAsync());
            if (responseData != null && responseData.data != null && responseData.data.Count > 0)
            {
                var type = responseData.data[0].brand == "HUMOCARD" ? "Humo" :
                     responseData.data[0].brand == "VISA" ? "Visa" :
                     responseData.data[0].brand == "CHINA UNION PAY" ? "UnionPay" :
                     responseData.data[0].brand == "MASTERCARD" ? "MasterCard" : "Uzcard"
                     ;
                prefix = new CardPrefixApi
                {
                    Prefix = uint.Parse(responseData.data[0].bin),
                    BankName = responseData.data[0].bank ?? "unknown",
                    CardType = type
                };
                await commander.Call(new CreateCardPrefixCommand(new Session("~"), new CardPrefixView
                {
                    Prefix = uint.Parse(responseData.data[0].bin),
                    BankName = responseData.data[0].bank ?? "unknown",
                    CardType = type,
                }), cancellationToken: cancellationToken);
            }
            else
            {
                logger.LogInformation(" card system not found in database, and no data from external API. {cardNumber}",cardNumber);
                prefix = new CardPrefixApi
                {
                    Prefix = uint.Parse(cardNumber.Substring(0, 6)), // Возвращаем первые 6 цифр как префикс
                    BankName = "unknown",
                    CardBrand = null,
                    CardType = "unknown"
                };
            }


        }


        return prefix ?? throw new ValidationException(
            "Card prefix not found for the provided card number.");
    }

    [ComputeMethod]
    public async virtual Task<CardPrefixView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var cardprefix = await dbContext.CardPrefixes
            .Include(x => x.CardBrand)
            .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken)
            ?? throw new ValidationException("CardPrefixEntity Not Found");

        return cardprefix.MapToView();
    }

    #endregion

    #region Mutations

    public async virtual Task Create(CreateCardPrefixCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        CardPrefixEntity cardprefix = new();
        Reattach(cardprefix, command.Entity, dbContext);

        dbContext.Update(cardprefix);
        await dbContext.SaveChangesAsync(cancellationToken);

    }

    public async virtual Task Update(UpdateCardPrefixCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var cardprefix = await dbContext.CardPrefixes
            .Include(x => x.CardBrand)
            .FirstOrDefaultAsync(x => x.Id == command.Entity.Id, cancellationToken)
            ?? throw new ValidationException("CardPrefixEntity Not Found");

        Reattach(cardprefix, command.Entity, dbContext);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async virtual Task Delete(DeleteCardPrefixCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var cardprefix = await dbContext.CardPrefixes
            .Include(x => x.CardBrand)
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
            ?? throw new ValidationException("CardPrefixEntity Not Found");
        dbContext.Remove(cardprefix);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    private static void Reattach(CardPrefixEntity cardprefix, CardPrefixView cardprefixView, AppDbContext dbContext)
    {
        CardPrefixMapper.From(cardprefixView, cardprefix);

        if (cardprefix.CardBrand != null)
            cardprefix.CardBrand = dbContext.Files
            .First(x => x.Id == cardprefix.CardBrand.Id);
    }

    private static void Sorting(ref IQueryable<CardPrefixEntity> cardprefix, TableOptions options)
        => cardprefix = options.SortLabel switch
        {
            "Prefix" => cardprefix.Ordering(options, o => o.Prefix),
            "BankName" => cardprefix.Ordering(options, o => o.BankName),
            "CardType" => cardprefix.Ordering(options, o => o.CardType),
            "CardBrand" => cardprefix.Ordering(options, o => o.CardBrand),
            "Id" => cardprefix.Ordering(options, o => o.Id),
            _ => cardprefix.OrderBy(o => o.Id),

        };



    #endregion
}

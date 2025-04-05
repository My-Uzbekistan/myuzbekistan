using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;
using myuzbekistan.Shared;

namespace myuzbekistan.Services;

public class CurrencyInvalidateScheduler: IInvocable
{
    private readonly ICurrencyService _currencyService;
    private readonly AppDbContext _appDbContext;

    public CurrencyInvalidateScheduler(ICurrencyService currencyService, IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _currencyService = currencyService;
        _appDbContext = dbContextFactory.CreateDbContext();

    }
    public async Task Invoke()
    {
        await _currencyService.Invalidate();

        var currency = await _currencyService.GetUsdCourse();
        decimal rate = decimal.Parse(currency.Rate.Replace(".", ","));

        await _appDbContext.Database.ExecuteSqlRawAsync(
            "UPDATE \"Contents\" SET \"PriceInDollar\" = ceil(\"Price\" / {0}) WHERE \"Price\" != 0",
            rate
        );



    }
}

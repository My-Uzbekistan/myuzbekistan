using Coravel.Invocable;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using myuzbekistan.Shared;
using System.Globalization;

namespace myuzbekistan.Services;

public class CurrencyInvalidateScheduler: IInvocable
{
    private readonly ICurrencyService _currencyService;
    private readonly ILogger<CurrencyInvalidateScheduler> _logger;
    private readonly AppDbContext _appDbContext;

    public CurrencyInvalidateScheduler(ICurrencyService currencyService, IDbContextFactory<AppDbContext> dbContextFactory,ILogger<CurrencyInvalidateScheduler> logger)
    {
        _currencyService = currencyService;
        this._logger = logger;
        _appDbContext = dbContextFactory.CreateDbContext();

    }
    public async Task Invoke()
    {
        await _currencyService.Invalidate();

        var currency = await _currencyService.GetUsdCourse();
        decimal rate = decimal.Parse(currency.Rate, CultureInfo.InvariantCulture);

        _logger.LogWarning($"CurrencyInvalidateScheduler: {currency.Ccy} - {currency.Rate}");   

        await _appDbContext.Database.ExecuteSqlRawAsync(
            "UPDATE \"Contents\" SET \"PriceInDollar\" = ceil(\"Price\" / {0}) WHERE \"Price\" != 0",
            rate
        );



    }
}

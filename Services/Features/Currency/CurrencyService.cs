using ActualLab.Async;
using ActualLab.Fusion;
using ActualLab.Fusion.EntityFramework;
using myuzbekistan.Shared;
using System.Net.Http.Json;
using System.Reactive;
namespace myuzbekistan.Services;

public class CurrencyService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), ICurrencyService
{
    #region Queries
    [ComputeMethod]
    public async virtual Task<List<Currency>> GetCurrencies(CancellationToken cancellationToken = default)
    {
        await Invalidate();
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        var date = DateTime.Now.ToString("YYYY-MM-DD");
        var response = await client.GetFromJsonAsync<List<CurrencyRaw>>($"https://cbu.uz/ru/arkhiv-kursov-valyut/json/all/{date}/", cancellationToken: cancellationToken);
        return response.MapToViewList() ?? [];
    }

    [ComputeMethod]
    public async virtual Task<Currency> GetUsdCourse(CancellationToken cancellationToken = default)
    {
        var currencies = await GetCurrencies(cancellationToken);

        return currencies.FirstOrDefault(x => x.Ccy == "USD") ?? new Currency();   
    }



    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
   
    #endregion
}

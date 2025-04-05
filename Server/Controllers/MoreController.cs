using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Services;
using myuzbekistan.Shared;

namespace Server.Controllers;

[Route("api/more")]
[ApiController]
public class MoreController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IContentService _contentService;
    private readonly ICurrencyService _currencyService;


    public MoreController(ICategoryService categoryService, IContentService contentService, ICurrencyService currencyService)
    {
        _categoryService = categoryService;
        _contentService = contentService;
        _currencyService = currencyService;
    }

    [HttpGet("about")]
    public async Task<List<ContentShort>> GetAbout([FromQuery] TableOptions tableOptions, CancellationToken cancellationToken)
    {
        return await _contentService.GetContentByCategoryName("About Uzbekistan", cancellationToken);
    }

    [HttpGet("useful")]
    public async Task<List<ContentShort>> GetUseful(CancellationToken cancellationToken)
    {
        return await _contentService.GetContentByCategoryName("Useful tips", cancellationToken);
    }

    [HttpGet("currency")]
    public async Task<List<Currency>> GetCurrency(CancellationToken cancellationToken)
    {
        return await _currencyService.GetCurrencies(cancellationToken);
    }
}


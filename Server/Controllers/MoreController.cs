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


    public MoreController(ICategoryService categoryService, IContentService contentService)
    {
        _categoryService = categoryService;
        _contentService = contentService;
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
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        var date = DateTime.Now.ToString("YYYY-MM-DD");
        var response = await client.GetFromJsonAsync<List<Currency>>($"https://cbu.uz/ru/arkhiv-kursov-valyut/json/all/{date}/", cancellationToken: cancellationToken);
        return response ?? [];
    }
}


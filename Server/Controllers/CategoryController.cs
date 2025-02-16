using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Services;
using myuzbekistan.Shared;

namespace Server.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;

    [HttpGet]
    public async Task<List<CategoryApi>> GetCategories([FromQuery] TableOptions tableOptions, CancellationToken cancellationToken)
    {
        return await _categoryService.GetCategories(cancellationToken);
    }

    [HttpGet("main-page")]
    public async Task<List<MainPageApi>> GetMainPage([FromQuery] TableOptions tableOptions, CancellationToken cancellationToken)
    {
        return await _categoryService.GetMainPageApi(cancellationToken);
    }
}


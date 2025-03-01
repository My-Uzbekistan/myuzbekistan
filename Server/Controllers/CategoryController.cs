using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Services;
using myuzbekistan.Shared;

namespace Server.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoryController(ICategoryService categoryService, IContentService contentService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;
    private readonly IContentService _contentService = contentService;

    [HttpGet]
    public async Task<List<CategoryApi>> GetCategories([FromQuery] TableOptions tableOptions, CancellationToken cancellationToken)
    {
        return await _categoryService.GetCategories(cancellationToken);
    }

    [HttpGet("main-page")]
    public async Task<List<MainPageApi>> GetMainPage([FromQuery] TableOptions tableOptions, CancellationToken cancellationToken)
    {
        return await _categoryService.GetMainPageApi(tableOptions, cancellationToken);
    }

    [HttpGet("{id}/contents")]
    public async Task<List<ContentApiView>> GetContents(long id, [FromQuery] TableOptions tableOptions, CancellationToken cancellationToken)
    {
        return await _contentService.GetContents(id, tableOptions, cancellationToken);
    }

}


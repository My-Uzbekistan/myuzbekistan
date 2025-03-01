using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Services;
using myuzbekistan.Shared;

namespace Server.Controllers;

[Route("api/contents")]
[ApiController]
public class ContentController(ICategoryService categoryService, IContentService contentService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;
    private readonly IContentService _contentService = contentService;

    [HttpGet("{contentId}")]
    public async Task<ContentDto> GetContent(long contentId, CancellationToken cancellationToken)
    {
        return await _contentService.GetContent(contentId, cancellationToken);
    }
}


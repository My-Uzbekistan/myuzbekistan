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
    public async Task<IActionResult> GetContent(long contentId, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _contentService.GetContent(contentId, cancellationToken));
        }
        catch (NotFoundException)
        {

            return NotFound();
        }
        
    }
}


using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Services;
using myuzbekistan.Shared;
using System.Security.Claims;

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
            var userId= User.Id();
            return Ok(await _contentService.GetContent(contentId, userId, cancellationToken));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}


using ActualLab.CommandR;
using ActualLab.Fusion;
using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Services;
using myuzbekistan.Shared;
using System.Globalization;
using System.Security.Claims;

namespace Server.Controllers;

[Route("api/contents")]
[ApiController]
public class ContentController(ICategoryService categoryService, IContentService contentService, ISessionResolver sessionResolver, ICommander commander) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;
    private readonly IContentService _contentService = contentService;

    [HttpGet("{contentId}")]
    public async Task<IActionResult> GetContent(long contentId, CancellationToken cancellationToken)
    {
        ContentDto? contentDto = null;
        try
        {
            var userId = User.Id();
            contentDto = await _contentService.GetContent(contentId, userId, cancellationToken);
            return Ok(contentDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        finally
        {
            var contents = await _contentService.Get(contentId, cancellationToken);
            var content = contents.FirstOrDefault(x => x.Locale == LangHelper.currentLocale);
            if(content != null)
            await commander.Call(new AddRequestCommand(sessionResolver.Session, new ContentRequestView
            {
                ContentId = contentId,
                ContentName = contentDto?.Title ?? string.Empty,
                UserId = User.Id(),
                ContentLocale = LangHelper.currentLocale,
                RegionId = content?.RegionId,
                RegionName = content?.RegionView?.Name,
                CategoryId = content?.CategoryId ?? 0, // Fix for CS0266 and CS8629
                CategoryName = content?.CategoryView?.Name
            }), cancellationToken: cancellationToken);
        }
    }
}


using ActualLab.CommandR;
using ActualLab.Fusion;
using DocumentFormat.OpenXml.Office2019.Word.Cid;
using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Services;
using myuzbekistan.Shared;
using System.Security.Claims;

namespace Server.Controllers;

[Route("api/v2/categories")]
[ApiController]
public class V2CategoryController(ICategoryService categoryService, IContentService contentService,ISessionResolver sessionResolver, ICommander commander) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;
    private readonly IContentService _contentService = contentService;

    [HttpGet]
    public async Task<List<CategoryApi>> GetCategories([FromQuery] TableOptions tableOptions, CancellationToken cancellationToken)
    {
        return await _categoryService.GetCategories(cancellationToken,true);
    }

    [HttpGet("main-page")]
    public async Task<List<MainPageApi>> GetMainPage([FromQuery] TableOptions tableOptions, CancellationToken cancellationToken)
    {
        return await _categoryService.GetMainPageApi(tableOptions, cancellationToken,true);
    }

    [HttpGet("{id}/contents")]
    public async Task<List<MainPageContent>> GetContents(long id, [FromQuery] TableOptions tableOptions, CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            long.TryParse(userId, out long userIdLong);
            return await _contentService.GetContents(id, userIdLong, tableOptions, cancellationToken,true);
        }
        
        finally
        {
            var categories = await categoryService.Get(id, cancellationToken);
            var category = categories.First(x => x.Locale == LangHelper.currentLocale);

            await commander.Call(new AddRequestCommand(sessionResolver.Session, new ContentRequestView
            {
                UserId = User.Id(),
                ContentLocale = LangHelper.currentLocale,
                CategoryId = category.Id,
                CategoryName = category.Name
            }), cancellationToken: cancellationToken);
        }
        
    }

}


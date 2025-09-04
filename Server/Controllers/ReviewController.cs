using ActualLab.CommandR;
using ActualLab.Fusion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Shared;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReviewController(IReviewService reviewService, ISessionResolver sessionResolver, ICommander commander) : ControllerBase
    {
        // POST api/reviews
        // Body: { "contentId": 1, "comment": "text", "rating": 5 }
        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] CreateReviewDto dto, CancellationToken cancellationToken)
        {
            if (dto is null) return BadRequest();
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var session = await sessionResolver.GetSession();
            try
            {
                await commander.Call(new CreateReviewCommand(session, new ReviewView
                {
                    Rating = dto.Rating,
                    UserId = userId,
                    ContentEntityId = dto.ContentId,
                    Comment = dto.Comment
                }), cancellationToken);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        // GET api/reviews/content/{contentId}?page=1&pageSize=15
        [HttpGet("content/{contentId:long}")]
        [AllowAnonymous]
        public async Task<ActionResult<TableResponse<ReviewView>>> GetByContent(long contentId, [FromQuery] int page = 1, [FromQuery] int pageSize = 15, [FromQuery] string? sortLabel = null, [FromQuery] int sortDirection = 1, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 15;
            var options = new TableOptions { Page = page, PageSize = pageSize, SortLabel = sortLabel, SortDirection = sortDirection };
            var result = await reviewService.GetByContent(contentId, options, cancellationToken);
            return Ok(result);
        }
    }
}

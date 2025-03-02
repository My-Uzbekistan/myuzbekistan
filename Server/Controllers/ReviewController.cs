using ActualLab.CommandR;
using ActualLab.Fusion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Shared;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ReviewController(IReviewService reviewService, ISessionResolver sessionResolver, ICommander commander) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> AddReview([FromQuery] long contentId, [FromQuery] string? comment ,[FromQuery] int rating)
        {
            var userId = long.Parse(HttpContext.User.Identities.First().Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var session = await sessionResolver.GetSession();
            try
            {
                await commander.Call(new CreateReviewCommand(session, new ReviewView
                {
                    Rating = rating,
                    UserId = userId,
                    ContentEntityId = contentId
                }));
                return Ok();
            }
            catch (NotFoundException)
            {

                return NotFound();
            }
            
        }


    }
}

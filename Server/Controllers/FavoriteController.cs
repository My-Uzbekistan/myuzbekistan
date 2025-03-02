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
    [Route("api/favorites")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class FavoriteController(IFavoriteService favoriteService, ISessionResolver sessionResolver, ICommander commander) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromQuery] long contentId)
        {
            var userId = long.Parse(HttpContext.User.Identities.First().Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var session = await sessionResolver.GetSession();
            try
            {
                await commander.Call(new CreateFavoriteCommand(session, contentId, userId));
                return Ok();
            }
            catch (NotFoundException)
            {

                return NotFound();
            }
            
        }

        [HttpDelete]

        public async Task<IActionResult> UnFavorite([FromQuery] long favoriteId)
        {
            var userId = long.Parse(HttpContext.User.Identities.First().Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var session = await sessionResolver.GetSession();
            await commander.Call(new DeleteFavoriteCommand(session,  favoriteId, userId));
            return Ok();
        }

        [HttpGet]
        public async Task<List<FavoriteApiView>> GetFavorites(CancellationToken cancellationToken)
        {
            var userId = long.Parse(HttpContext.User.Identities.First().Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var session = await sessionResolver.GetSession();
            return await favoriteService.GetFavorites(userId, cancellationToken);
        }

    }
}

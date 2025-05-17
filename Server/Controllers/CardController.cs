using ActualLab.CommandR;
using ActualLab.Fusion;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Services;
using myuzbekistan.Shared;

namespace Server.Controllers
{
    [Route("api/cards")]
    [ApiController]
    [Authorize]
    public class CardController(ICardService cardService, MultiCardService multiCardService, ICommander commander, ISessionResolver sessionResolver) : ControllerBase
    {

        [HttpPost("bind-card")]
        public async Task<IActionResult> BindCard([FromBody] MultiBindCardRequest request)
        {
            var userId = User.Id();
            var session = await sessionResolver.GetSession();
            var cardInfo = await multiCardService.BindCard(request);
            var res = await commander.Call(new CreateCardCommand(session, new CardView { UserId = userId, CardToken = cardInfo.CardToken, ExpirationDate = request.Expiry }));

            return Ok(res);
        }

        [HttpPost("confirm-card/{id:long}")]
        public async Task<IActionResult> ConfirmCard([FromRoute] long Id, [FromBody] MultiConfirmCardRequest request, CancellationToken cancellationToken )
        {
            var userId = User.Id();

            var session = await sessionResolver.GetSession();

            var cardInfo = await cardService.Get(Id,userId, cancellationToken);
            var confirmedCard = await multiCardService.ConfirmBindCard(cardInfo.CardToken, request.Otp);
            confirmedCard.CardId = confirmedCard.Id;
            confirmedCard.Id = cardInfo.Id;
            confirmedCard.UserId = userId;
            confirmedCard.ExpirationDate = cardInfo.ExpirationDate;
            await commander.Call(new UpdateCardCommand(session, confirmedCard));

            return Ok();
        }

        [HttpGet("list-card")]
        public async Task<IActionResult> ListCard()
        {
            var userId = User.Id();
            var cards = await cardService.GetCardByUserId(userId);
            return Ok(cards);
        }

    }
}

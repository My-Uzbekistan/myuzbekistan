using ActualLab.CommandR;
using ActualLab.Fusion;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using myuzbekistan.Services;
using myuzbekistan.Shared;

namespace Server.Controllers
{
    [Route("api/cards")]
    [ApiController]
    [Authorize]
    public class CardController(ICardService cardService, ICardPrefixService cardPrefixService,ICardColorService cardColorService , MultiCardService multiCardService, ICommander commander, ISessionResolver sessionResolver) : ControllerBase
    {

        [HttpPost("bind-card")]
        public async Task<IActionResult> BindCard([FromBody] MultiBindCardRequest request, CancellationToken cancellationToken)
        {
            var cardPrefix = await cardPrefixService.GetTypeByCardNumber(request.Token!, cancellationToken);
            if ((cardPrefix.CardType == "Visa" || cardPrefix.CardType == "MasterCard") && request.Cvv is null)
            {
                throw new Exception("CVV is required for Visa and MasterCard cards.");
            }
            var userId = User.Id();
            var session = await sessionResolver.GetSession();
            var card = await cardService.CheckCard(userId, request.Token!, cancellationToken);
            if (card)
            {
                throw new Exception("Card already exists.");
            }
            var cardInfo = await multiCardService.BindCard(request);
            var res = await commander.Call(new CreateCardCommand(session, new CardView { UserId = userId, CardToken = cardInfo.CardToken, ExpirationDate = request.Expiry, Code = new CardColorView {  Id = request.CardColorId } , Cvv = request.Cvv, Name = request.Name }), cancellationToken: cancellationToken);

            return Ok(new { cardId = res } );
        }


        [HttpGet("card-type")]
        public async Task<IActionResult> CardType([FromQuery] string cardNumber, CancellationToken cancellationToken = default)
        {
            return Ok(await cardPrefixService.GetTypeByCardNumber(cardNumber));
        }

        [HttpGet("card-colors")]
        public async Task<IActionResult> CardColor(CancellationToken cancellationToken = default)
        {
            return Ok(await cardColorService.GetAll(new TableOptions { Page = 1 , PageSize = 200}));
        }

        [HttpPost("confirm-card/{id:long}")]
        public async Task<IActionResult> ConfirmCard([FromRoute] long Id, [FromBody] MultiConfirmCardRequest request, CancellationToken cancellationToken)
        {
            var userId = User.Id();

            var session = await sessionResolver.GetSession(cancellationToken);

            var cardInfo = await cardService.Get(Id, userId, cancellationToken);
            var confirmedCard = await multiCardService.ConfirmBindCard(cardInfo.CardToken, request.Otp);
            confirmedCard.CardId = confirmedCard.Id;
            confirmedCard.Id = cardInfo.Id;
            confirmedCard.UserId = userId;
            confirmedCard.ExpirationDate = cardInfo.ExpirationDate;
            confirmedCard.Name = cardInfo.Name;
            confirmedCard.Code = cardInfo.Code;
            confirmedCard.Cvv = cardInfo.Cvv;
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

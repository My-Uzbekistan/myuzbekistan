using myuzbekistan.Services;

namespace Server.Controllers
{
    [Route("api/cards")]
    [ApiController]
    [Authorize]
    public class CardController(ICardService cardService, ICardPrefixService cardPrefixService, ICardColorService cardColorService, GlobalPayService globalPayService, ICommander commander, ISessionResolver sessionResolver) : ControllerBase
    {

        public static string MaskCardNumber(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 10)
                throw new ArgumentException("Card number must be at least 10 characters long.");

            var firstSix = cardNumber.Substring(0, 6);
            var lastFour = cardNumber.Substring(cardNumber.Length - 4);
            var maskedMiddle = new string('*', cardNumber.Length - 10);

            return $"{firstSix}{maskedMiddle}{lastFour}";
        }

        [HttpPost("bind-card")]
        public async Task<IActionResult> BindCard([FromBody] PaymentVendorCardRequest request, CancellationToken cancellationToken)
        {
            var cardPrefix = await cardPrefixService.GetTypeByCardNumber(request.Token!, cancellationToken);
            var extCardTypes = new[] { "Visa", "MasterCard" };

            var userId = User.Id();
            var session = await sessionResolver.GetSession(cancellationToken);
            string token = request.Token.Contains("*") ? request.Token! : MaskCardNumber(request.Token);
            var card = await cardService.GetByPan(userId, request.Token, cancellationToken);

            if (card != null && card.Status == "active")
            {
                throw new MyUzException("CardAlreadyExists");
            }

            if (extCardTypes.Contains(cardPrefix.CardType) && string.IsNullOrEmpty(request.CardHolderName))
            {
                throw new MyUzException("CardHolderNameRequired");
            }
            else if (extCardTypes.Contains(cardPrefix.CardType) && (request.Cvv == null || request.Cvv == 0))
            {
                throw new MyUzException("CvvRequired");

            }
            else if (extCardTypes.Contains(cardPrefix.CardType) && !string.IsNullOrEmpty(request.SmsNotificationNumber))
            {
                throw new MyUzException("SmsNotificationNumberMustBeNullForExternalCards");

            }

            else if (!extCardTypes.Contains(cardPrefix.CardType) && string.IsNullOrEmpty(request.SmsNotificationNumber))
            {
                throw new MyUzException("SmsNotificationNumberRequired");

            }

            var cardInfo = await globalPayService.BindCard(new GPBindCardRequest
            {
                CardHolderName = request.CardHolderName,
                SmsNotificationNumber = request.SmsNotificationNumber,
                CardNumber = request.Token!.Replace(" ", ""),
                ExpiryDate = request.Expiry,
            });

            var res = card?.Id;
            var createCard = new CardView
            {
                UserId = userId,
                CardToken = cardInfo.CardToken,
                Icon = request.Image,
                ExpirationDate = request.Expiry,
                CardPan = MaskCardNumber(request.Token),
                Cvv = request.Cvv,
                Ps = cardInfo.ProcessingType,
                Phone = request.SmsNotificationNumber,
                HolderName = request.CardHolderName,
                Status = extCardTypes.Contains(cardPrefix.CardType) ? "active" : null
            };
            if (card != null)
            {
                createCard.Id = card.Id;
                await commander.Call(new UpdateCardCommand(session, createCard), cancellationToken: cancellationToken);
            }
            else
            {
                res = await commander.Call(new CreateCardCommand(session, createCard), cancellationToken: cancellationToken);

            }



            return Ok(new { cardId = res });
        }


        [HttpGet("card-type")]
        public async Task<IActionResult> CardType([FromQuery] string cardNumber, CancellationToken cancellationToken = default)
        {
            return Ok(await cardPrefixService.GetTypeByCardNumber(cardNumber, cancellationToken));
        }

        [HttpGet("card-colors")]
        public async Task<IActionResult> CardColor(CancellationToken cancellationToken = default)
        {
            return Ok(await cardColorService.GetAllApi(new TableOptions { Page = 1, PageSize = 200 }));
        }

        [HttpPost("confirm-card/{id:long}")]    
        public async Task<IActionResult> ConfirmCard([FromRoute] long Id, [FromBody] MultiConfirmCardRequest request, CancellationToken cancellationToken)
        {
            var userId = User.Id();

            var session = await sessionResolver.GetSession(cancellationToken);

            var cardInfo = await cardService.Get(Id, userId, cancellationToken);
            var confirmedCard = await globalPayService.ConfirmBindCard(cardInfo.CardToken, request.Otp);
            confirmedCard.CardId = confirmedCard.Id;
            confirmedCard.Id = cardInfo.Id;
            confirmedCard.UserId = userId;
            confirmedCard.ExpirationDate = cardInfo.ExpirationDate;
            confirmedCard.Ps = cardInfo.Ps;
            confirmedCard.HolderName = cardInfo.HolderName;
            confirmedCard.Status = "active";
            confirmedCard.Phone = cardInfo.Phone;
            confirmedCard.Icon = cardInfo.Icon;
            await commander.Call(new UpdateCardCommand(session, confirmedCard), cancellationToken: cancellationToken);

            return Ok();
        }

        [HttpGet("list-card")]
        public async Task<IActionResult> ListCard()
        {
            var userId = User.Id();
            var cards = await cardService.GetCardByUserId(userId);
            return Ok(cards);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteCard([FromRoute] long id, CancellationToken cancellationToken)
        {
            var userId = User.Id();
            var session = await sessionResolver.GetSession(cancellationToken);
            await commander.Call(new DeleteCardCommand(session, id, userId), cancellationToken: cancellationToken);
            return NoContent();
        }

    }
}

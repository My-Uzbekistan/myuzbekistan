using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Services;
using myuzbekistan.Shared;

namespace Server.Controllers;

[Route("api/payments")]
[Authorize]
[ApiController]
public class PaymentController(MultiCardService multiCardService, ICardService cardService) : ControllerBase
{

    [HttpPost("top-up")]
    public async Task<IActionResult> TopUp([FromBody] TopUpRequest topUp, CancellationToken cancellationToken)
    {
        if (topUp.Amount <= 0)
        {
            return BadRequest(new { Message = "Amount must be greater than 0" });
        }
        var userId = User.Id();
        var card = await cardService.Get(topUp.CardId, userId,cancellationToken);
        var res = await multiCardService.CreatePayment(userId, topUp.Amount, card.CardToken);
        return Ok(new { Message = res });
    }

    [HttpPost("confirm-top-up")]
    public async Task<IActionResult> ConfirmTopUp([FromBody] ConfirmTopUpRequest confirmTopUp)
    {
        long userId = User.Id();
        await multiCardService.ConfirmPayment(confirmTopUp.PaymentId, confirmTopUp.Otp);
        return Ok();
    }

}

public class ConfirmTopUpRequest
{
    public string Otp { get; set; } = null!;
    public string PaymentId { get; set; } = null!;
}

public class TopUpRequest
{
    public long Amount { get; set; }
    public long CardId { get; set; }
}
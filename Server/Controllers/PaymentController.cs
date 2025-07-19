using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Services;
using myuzbekistan.Shared;

namespace Server.Controllers;

[Route("api/payments")]
[Authorize]
[ApiController]
public class PaymentController(GlobalPayService globalPayService, ICardService cardService) : ControllerBase
{

    [HttpPost("top-up")]
    public async Task<IActionResult> TopUp([FromBody] TopUpRequest topUp, CancellationToken cancellationToken)
    {
        if (topUp.Amount <= 0)
        {
            throw new MyUzException("AmountMustBeGreaterThanZero");
        }
        topUp.Amount *=100;
        var userId = User.Id();
        var card = await cardService.Get(topUp.CardId, userId, cancellationToken);
        var res = await globalPayService.CreatePayment(userId, topUp.Amount, card);
        return Ok(new { PaymentId = res.ExternalId });
    }

    [HttpPost("confirm-top-up")]
    public async Task<IActionResult> ConfirmTopUp([FromBody] ConfirmTopUpRequest confirmTopUp)
    {
        long userId = User.Id();
        await globalPayService.ConfirmPayment(confirmTopUp.PaymentId);
        return Ok();
    }

}

public class ConfirmTopUpRequest
{
    public string PaymentId { get; set; } = null!;
}

public class TopUpRequest
{
    public long Amount { get; set; }
    public long CardId { get; set; }
}
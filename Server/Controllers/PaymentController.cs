using ActualLab.Fusion.Authentication;
using Microsoft.Extensions.Localization;
using myuzbekistan.Services;

namespace Server.Controllers;

[Route("api/payments")]
[Authorize]
[ApiController]
public class PaymentController(GlobalPayService globalPayService, ICardService cardService,ICommander _commander, IMerchantService merchantService,IInvoiceService invoiceService, IStringLocalizer<PaymentController> @L) : ControllerBase
{

    [HttpPost("top-up")]
    public async Task<IActionResult> TopUp([FromBody] TopUpRequest topUp, CancellationToken cancellationToken)
    {
        if (topUp.Amount <= 0)
        {
            throw new MyUzException("AmountMustBeGreaterThanZero");
        }
        topUp.Amount *=100;
        var sessionId = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("session"))?.Value;
        var sessionInfo = new ActualLab.Fusion.Session(sessionId);
        var userId = User.Id();
        var card = await cardService.Get(topUp.CardId, userId, cancellationToken);
        var res = await globalPayService.CreatePayment(userId, topUp.Amount, card);
        var  confirm = await globalPayService.ConfirmPayment(res.ExternalId);

        var merchant = await merchantService.Get(topUp.ServiceId, cancellationToken);

        if (card.Ps != "VISA" || card.Ps != "MasterCard")
        {
            var command = new CreateInvoiceCommand(sessionInfo, new InvoiceRequest
            {
                Amount = topUp.Amount,
                Description = $"payed for {merchant.First().Name}",
                MerchantId = topUp.ServiceId,
                PaymentId = res.ExternalId
            });
            await _commander.Call(command, cancellationToken);
        }
        
        return Ok(new { PaymentId = res.ExternalId, CheckUrl = confirm.SecurityCheckUrl });
    }


    [HttpGet]
    public async Task<IActionResult> Payments([FromQuery] TableOptions tableOptions,CancellationToken cancellationToken)
    {
        var userId = User.Id();
        // Get the invoices by user ID
        var invoices = await invoiceService.GetByPayments(tableOptions, userId, cancellationToken);

        // Return the response sorted by date in descending order
        return Ok(invoices);
    }



    [HttpGet("check/{paymentId}")]
    public async Task<IActionResult> Check(string PaymentId, CancellationToken cancellationToken)
    {
        // Get the invoice by payment ID
        var invoice = await invoiceService.GetByPaymentId(PaymentId, cancellationToken);
        if (invoice == null)
        {
            return NotFound(new { Message = "Invoice not found." });
        }

        
        

        // Prepare the items for the response
        var Items = new List<object>()
        {
            new {
               Key = @L["Transaction"].Value,
               Value = PaymentId
            },
            new {
               Key = @L["PaymentDate"].Value,
               Value = invoice.Date
            }
        };
        invoice.Items = Items;

        return Ok(invoice);

        
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
    public long ServiceId { get; set; }

    public string? Description { get; set; }
}
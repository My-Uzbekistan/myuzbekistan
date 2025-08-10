using ActualLab.Fusion.Authentication;
using Microsoft.Extensions.Localization;
using myuzbekistan.Services;
using System.Globalization;

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

        var command = new CreateInvoiceCommand(sessionInfo, new InvoiceRequest
        {
            Amount = topUp.Amount,
            Description = $"payed for {merchant.First().Name}",
            MerchantId = topUp.ServiceId,
            PaymentId = res.Id
        });
        await _commander.Call(command, cancellationToken);
        return Ok(new { PaymentId = res.ExternalId, CheckUrl = confirm.SecurityCheckUrl });
    }


    [HttpGet]
    public async Task<IActionResult> Payments(CancellationToken cancellationToken)
    {
        var userId = User.Id();
        // Get the invoices by user ID
        var invoices = await invoiceService.GetByPayments(userId, cancellationToken);
        if (invoices == null || !invoices.Any())
        {
            return NotFound(new { Message = "Invoices not found." });
        }

        // Group invoices by date
        var groupedInvoices = invoices
            .GroupBy(invoice => invoice.CreatedAt.Date)
            .OrderByDescending(group => group.Key)
            .ToDictionary(
                group => group.Key.ToString("dd MMMM", CultureInfo.CurrentCulture),
                group => group.Select(invoice => new
                {
                    MerchantIcon =  invoice.MerchantView?.LogoView?.GetPreviewOrIcon(Constants.MinioPath),
                    MerchantName = invoice.MerchantView?.Name,
                    MerchantType = invoice.MerchantView?.MerchantCategoryView?.ServiceType.Name,
                    Amount = $"-{invoice.Amount/100:N0}"
                }).ToList()
            );

        // Return the response
        return Ok(groupedInvoices);
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

        // Get the merchant by invoice's merchant ID
        var merchants = await merchantService.Get(invoice.MerchantView.Id, cancellationToken);
        if (merchants == null || !merchants.Any())
        {
            return NotFound(new { Message = "Merchant not found." });
        }

        // Find the merchant by locale
        var merchantByLocale = merchants.FirstOrDefault(x => x.Locale == LangHelper.currentLocale);
        if (merchantByLocale == null)
        {
            return NotFound(new { Message = "Merchant for the current locale not found." });
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
               Value = invoice.CreatedAt
            }
        };

        // Return the response
        return Ok(new {
            MerchantIcon = merchantByLocale.LogoView.GetPreviewOrIcon(Constants.MinioPath),
            MerchantName = merchantByLocale.Name,
            MerchantType  = merchantByLocale.MerchantCategoryView.ServiceType.Name,
            Amount = invoice.Amount,
            Items = Items
        });
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
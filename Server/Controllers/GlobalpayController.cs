using Microsoft.Extensions.Localization;
using System.Text.Json.Serialization;

public class GlobalPayResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    [JsonPropertyName("externalId")]
    public string ExternalId { get; set; } = null!;
}


namespace Server.Controllers
{
    [Route("api/global-pay")]
    [ApiController]
    public class GlobalPayController(ICommander Commander, IInvoiceService invoiceService, IStringLocalizer<PaymentController> @L, ISessionResolver sessionResolver) : ControllerBase
    {
        //[HttpPost("fail")]
        //public async Task<IActionResult> Success([FromQuery(Name = "externalId")] string externalId, CancellationToken cancellationToken)
        //{
        //    var invoiceDetail = await invoiceService.GetByPaymentId(externalId, cancellationToken);
        //    if (invoiceDetail == null)
        //    {
        //        return NotFound("Invoice not found for the provided payment ID.");
        //    }
        //    await Commander.Call(new ChangePaymentStateCommand(sessionResolver.Session, externalId, PaymentStatus.Failed), cancellationToken);
        //    await Commander.Call(new UpdateInvoiceStatusCommand(sessionResolver.Session, PaymentStatus.Failed, externalId), cancellationToken);
        //    return Ok();
        //}


        //На самом деле succes но так получилось
        [HttpPost("fail")]
        public async Task<IActionResult> Fail([FromBody] GlobalPayResponse data, CancellationToken cancellationToken)
        {
            var invoiceDetail = await invoiceService.GetByPaymentId(data.ExternalId, cancellationToken);
            if (invoiceDetail == null)
            {
                return NotFound("Invoice not found for the provided payment ID.");
            }

            await Commander.Call(new ChangePaymentStateCommand(sessionResolver.Session, data.ExternalId, PaymentStatus.Completed), cancellationToken);
            await Commander.Call(new UpdateInvoiceStatusCommand(sessionResolver.Session, PaymentStatus.Completed, data.ExternalId), cancellationToken);
            return Ok();
        }

    }
}

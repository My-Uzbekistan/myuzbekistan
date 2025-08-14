using Microsoft.Extensions.Localization;

namespace Server.Controllers
{
    [Route("api/global-pay")]
    [ApiController]
    public class GlobalPayController( ICommander Commander,IInvoiceService invoiceService, IStringLocalizer<PaymentController> @L, ISessionResolver sessionResolver) : ControllerBase
    {
        [HttpGet("fail")]
        public async Task<IActionResult> Fail([FromQuery(Name = "externalId")] string externalId, CancellationToken cancellationToken)
        {
            var invoiceDetail = await invoiceService.GetByPaymentId(externalId, cancellationToken);
            if (invoiceDetail == null)
            {
                return NotFound("Invoice not found for the provided payment ID.");
            }
            await Commander.Call(new ChangePaymentStateCommand(sessionResolver.Session, externalId,PaymentStatus.Failed), cancellationToken);
            await Commander.Call(new UpdateInvoiceStatusCommand(sessionResolver.Session, PaymentStatus.Failed, externalId), cancellationToken);
            return Ok();
        }

        [HttpGet("success")]
        public async Task<IActionResult> Success([FromQuery(Name = "externalId")] string externalId, CancellationToken cancellationToken)
        {
            var invoiceDetail = await invoiceService.GetByPaymentId(externalId, cancellationToken);
            if (invoiceDetail == null)
            {
                return NotFound("Invoice not found for the provided payment ID.");
            }

            await Commander.Call(new ChangePaymentStateCommand(sessionResolver.Session, externalId, PaymentStatus.Completed), cancellationToken);
            await Commander.Call(new UpdateInvoiceStatusCommand(sessionResolver.Session, PaymentStatus.Completed, externalId), cancellationToken);
            return Ok();
        }

    }
}

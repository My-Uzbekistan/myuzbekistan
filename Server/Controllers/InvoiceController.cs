using ActualLab.CommandR;
using ActualLab.Fusion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Shared;

namespace Server.Controllers
{
    [Route("api/invoices")]
    [ApiController]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ISessionResolver _sessionResolver;
        private readonly ICommander _commander;
        public InvoiceController(IInvoiceService invoiceService, ISessionResolver sessionResolver, ICommander commander)
        {
            _invoiceService = invoiceService;
            _sessionResolver = sessionResolver;
            _commander = commander;
        }
        [HttpGet]
        public async Task<TableResponse<InvoiceView>> GetInvoices([FromQuery] TableOptions tableOptions, CancellationToken cancellationToken)
        {
            return await _invoiceService.GetAll(tableOptions, cancellationToken);
        }
        [HttpGet("{id}")]
        public async Task<InvoiceView> GetInvoice(long id, CancellationToken cancellationToken)
        {
            return await _invoiceService.Get(id, cancellationToken);
        }
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceRequest request, CancellationToken cancellationToken)
        {
            var sessionId = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("session"))?.Value;
            var sessionInfo = new ActualLab.Fusion.Session(sessionId);
            var command = new CreateInvoiceCommand(sessionInfo, request);
            await _commander.Call(command, cancellationToken);
            return Ok();
        }
    }
}

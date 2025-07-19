using ActualLab.CommandR;
using ActualLab.Fusion;
using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Shared;

namespace Server.Controllers;

[Route("api/esim")]
[ApiController]
public class EsimController(
    IESimOrderService esimOrderService,
    ISessionResolver sessionResolver,
    ICommander commander) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
    {
        var session = await sessionResolver.GetSession(cancellationToken);
        if (session == null)
        {
            return Unauthorized();
        }
        var orders = await esimOrderService.GetAllEsim(options, session, cancellationToken);
        return Ok(orders);
    }

    [HttpGet("/{id}")]
    public async Task<IActionResult> Get(long id, CancellationToken cancellationToken = default)
    {
        var session = await sessionResolver.GetSession(cancellationToken);
        if (session == null)
        {
            return Unauthorized();
        }
        var orders = await esimOrderService.GetEsim(id, session, cancellationToken);
        return Ok(orders);
    }

    [HttpGet("/{id}/details")]
    public async Task<IActionResult> GetDetails(long id, CancellationToken cancellationToken = default)
    {
        var session = await sessionResolver.GetSession(cancellationToken);
        if (session == null)
        {
            return Unauthorized();
        }
        var order = await esimOrderService.Get(id, session, cancellationToken);
        using var httpClient = new HttpClient();
        var qrCodeBytes = await httpClient.GetByteArrayAsync(order.QrCode, cancellationToken);
        var base64 = Convert.ToBase64String(qrCodeBytes);

        var data = new
        {
            iccid = order.Iccid,
            smdpAddress = order.Lpa,
            activationCode = order.QrCode,
            qrCodeImageBase64 = base64
        };

        return Ok(order);
    }

    [HttpPost("order")]
    public async Task<IActionResult> MakeOrder([FromBody] CreateESimOrderView view, CancellationToken cancellationToken = default)
    {
        var session = await sessionResolver.GetSession(cancellationToken);
        if (session == null)
        {
            return Unauthorized();
        }
        var countries = await commander.Call(new MakeESimOrderCommand(session, view.PackageId), cancellationToken);
        return Ok(countries);
    }
}
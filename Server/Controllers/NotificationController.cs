using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Shared;
using ActualLab.CommandR;
using ActualLab.Fusion;

namespace Server.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationController(INotificationService service, ISessionResolver sessionResolver, ICommander commander) : ControllerBase
{
    [HttpGet]
    public async Task<TableResponse<NotificationView>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken ct = default)
    {
        var session = await sessionResolver.GetSession();
        var options = new TableOptions { Page = page, PageSize = pageSize, Search = search };
        return await service.GetAll(options, session, ct);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<NotificationView>> Get(long id, CancellationToken ct)
    {
        var session = await sessionResolver.GetSession();
        try { return await service.Get(id, session, ct); } catch (NotFoundException) { return NotFound(); }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NotificationView dto, CancellationToken ct)
    {
        var session = await sessionResolver.GetSession();
        await commander.Call(new CreateNotificationCommand(session, dto), ct);
        return Ok();
    }

    [HttpPost("{id:long}/seen")]
    public async Task<IActionResult> MarkSeen(long id, CancellationToken ct)
    {
        var session = await sessionResolver.GetSession();
        await commander.Call(new MarkNotificationSeenCommand(session, id), ct);
        return Ok();
    }

    [HttpPost("firebase-token")]
    public async Task<IActionResult> SetFirebaseToken([FromBody] SetFirebaseTokenRequest req, CancellationToken ct)
    {
        var session = await sessionResolver.GetSession();
        await commander.Call(new SetFirebaseTokenCommand(session, req.Token, req.OsVersion, req.Model, req.AppVersion), ct);
        return Ok();
    }
}

public record SetFirebaseTokenRequest(string Token, string OsVersion, string Model, string AppVersion);

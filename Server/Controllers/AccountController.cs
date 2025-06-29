using ActualLab.CommandR;
using ActualLab.Fusion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using myuzbekistan.Shared;

namespace Server.Controllers;

[Route("api/auth")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;
    private readonly ISessionResolver _sessionResolver;
    private readonly IUserService _userService;
    private readonly ICommander _commander;


    public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger<AuthController> logger, ISessionResolver sessionResolver, IUserService userService, ICommander commander )
    {
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
        _sessionResolver = sessionResolver;
        _userService = userService;
        _commander = commander;
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteUser([FromQuery] long id)
    {
        //var email = HttpContext.User.Claims.First(x => x.Type == "name").Value;
        //var user = await _userManager.FindByEmailAsync(email);
        var user = await _userManager.FindByIdAsync(id.ToString()) ;
        if (user == null)
        {
            return NotFound();
        }

        await _userManager.DeleteAsync(user);
        await _commander.Call(new InvalidateUserCommand(_sessionResolver.Session));

        return Ok();
    }
    [HttpGet("user-info")]

    public async Task<IActionResult> GetUserInfo()
    {
        
        var user = await _userManager.FindByIdAsync(User.Id().ToString());
        if (user == null)
        {
            return NotFound();
        }

        return Ok(new { Name = user.UserName, Balance = user.Balance});

    }


}



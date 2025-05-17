using ActualLab.CommandR;
using ActualLab.Fusion;
using AspNet.Security.OAuth.Apple;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using myuzbekistan.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
    public async Task<IActionResult> DeleteUser()
    {
        var email = HttpContext.User.Claims.First(x => x.Type == "name").Value;
        var user = await _userManager.FindByEmailAsync(email);
        if(user == null)
        {
            return NotFound();
        }

        await _commander.Call(new InvalidateUserCommand(_sessionResolver.Session));
        await _userManager.DeleteAsync(user);

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

        return Ok(new { Name = user.UserName, Balance = user.Balance / 100 });

    }


}


